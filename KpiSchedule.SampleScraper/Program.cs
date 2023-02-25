using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using KpiSchedule.Common.Mappers;
using AutoMapper;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Repositories;
using System.Collections.Concurrent;

Console.OutputEncoding = Encoding.UTF8;
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var serviceProvider = new ServiceCollection()
    .AddSerilogConsoleLogger(LogEventLevel.Verbose)
    .AddRozKpiParsers()
    .AddKpiClient<RozKpiApiTeachersClient>(config)
    .AddKpiClient<RozKpiApiGroupsClient>(config)
    .AddAutoMapper(typeof(RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile), typeof(RozKpiApiTeacherSchedule_TeacherScheduleEntity_MapperProfile))
    .AddDynamoDbSchedulesRepository<GroupSchedulesRepository, GroupScheduleEntity, GroupScheduleDayEntity, GroupSchedulePairEntity>(config)
    .AddDynamoDbSchedulesRepository<TeacherSchedulesRepository, TeacherScheduleEntity, TeacherScheduleDayEntity, TeacherSchedulePairEntity>(config)
    .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger>()!;
var mapper = serviceProvider.GetService<IMapper>()!;
var groupSchedulesRepository = serviceProvider.GetService<GroupSchedulesRepository>()!;
var teacherSchedulesRepository = serviceProvider.GetService<TeacherSchedulesRepository>()!;
var rozKpiApiGroupsClient = serviceProvider.GetRequiredService<RozKpiApiGroupsClient>()!;
var rozKpiApiTeachersClient = serviceProvider.GetRequiredService<RozKpiApiTeachersClient>()!;
var maxDegreeOfParallelism = 5;

var groupPrefixesToScrape = new[] { "а", "б", "в" }; // string or array of strings
var teacherPrefixesToScrape = new[] { "а", "б", "в" };

await ScrapeGroupSchedules(groupPrefixesToScrape);
await ScrapeTeacherSchedules(teacherPrefixesToScrape);

async Task ScrapeGroupSchedules(IEnumerable<string> prefixesToScrape)
{
    var groupNames = new ConcurrentBag<string>();
    await Parallel.ForEachAsync(prefixesToScrape, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
    }, async (prefix, token) =>
    {
        var groupNamesForPrefix = await rozKpiApiGroupsClient.GetGroups(prefix);
        foreach (var groupName in groupNamesForPrefix.Data)
        {
            groupNames.Add(groupName);
        }
    });

    var groupScheduleIds = new ConcurrentBag<Guid>();
    await Parallel.ForEachAsync(groupNames, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
    }, async (groupName, token) =>
    {
        try
        {
            var groupScheduleIdsForName = await rozKpiApiGroupsClient.GetGroupScheduleIds(groupName);
            foreach (var groupScheduleId in groupScheduleIdsForName)
            {
                groupScheduleIds.Add(groupScheduleId);
            }
        }
        catch (KpiScheduleClientGroupNotFoundException)
        {
            logger.Error("ScheduleId for group {groupName} not found", groupName);
        }
        catch
        {
            logger.Fatal("Unhandled error when getting scheduleId for {groupName}", groupName);
        }
    });

    int parserExceptionsCount = 0;
    int clientExceptionsCount = 0;
    int unhandledExceptionsCount = 0;

    var groupSchedules = new ConcurrentBag<RozKpiApiGroupSchedule>();
    await Parallel.ForEachAsync(groupScheduleIds, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism
    }, async (groupScheduleId, token) =>
    {
        try
        {
            var schedule = await rozKpiApiGroupsClient.GetGroupSchedule(groupScheduleId);
            groupSchedules.Add(schedule);
        }
        catch (KpiScheduleParserException)
        {
            Interlocked.Increment(ref parserExceptionsCount);
        }
        catch (KpiScheduleClientException)
        {
            Interlocked.Increment(ref clientExceptionsCount);
        }
        catch (Exception)
        {
            Interlocked.Increment(ref unhandledExceptionsCount);
            logger.Fatal("Caught an unhandled exception when trying to parse scheduleId {scheduleId}", groupScheduleId);
        }
    });

    var options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
    };

    logger.Information("Total exceptions caught during parsing: {parserExceptionsCount} during parsing, {clientExceptionsCount} from clients, {unhandledExceptionsCount} unhandled", parserExceptionsCount, clientExceptionsCount, unhandledExceptionsCount);
    logger.Information("Parsed a total of {schedulesCount} schedules, writing them to group-schedules.json", groupSchedules.Count);
    var schedulesJson = JsonSerializer.Serialize(groupSchedules, options);

    File.WriteAllText("group-schedules.json", schedulesJson);
}
async Task ScrapeTeacherSchedules(IEnumerable<string> prefixesToScrape)
{
    var teacherNames = new ConcurrentBag<string>();
    await Parallel.ForEachAsync(prefixesToScrape, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
    }, async (prefix, token) =>
    {
        var teacherNamesForPrefix = await rozKpiApiTeachersClient.GetTeachers(prefix);
        foreach (var teacherName in teacherNamesForPrefix.Data)
        {
            teacherNames.Add(teacherName);
        }
    });

    var teacherScheduleIds = new ConcurrentBag<Guid>();
    await Parallel.ForEachAsync(teacherNames, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
    }, async (teacherName, token) =>
    {
        try
        {
            var teacherScheduleId = await rozKpiApiTeachersClient.GetTeacherScheduleId(teacherName);
            teacherScheduleIds.Add(teacherScheduleId);
        }
        catch (KpiScheduleClientGroupNotFoundException)
        {
            logger.Error("ScheduleId for teacher {teacherName} not found", teacherName);
        }
        catch
        {
            logger.Fatal("Unhandled error when getting scheduleId for {teacherName}", teacherName);
        }
    });

    int parserExceptionsCount = 0;
    int clientExceptionsCount = 0;
    int unhandledExceptionsCount = 0;

    var teacherSchedules = new ConcurrentBag<RozKpiApiTeacherSchedule>();
    await Parallel.ForEachAsync(teacherScheduleIds, new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism
    }, async (teacherScheduleId, token) =>
    {
        try
        {
            var schedule = await rozKpiApiTeachersClient.GetTeacherSchedule(teacherScheduleId);
            teacherSchedules.Add(schedule);
        }
        catch (KpiScheduleParserException)
        {
            Interlocked.Increment(ref parserExceptionsCount);
        }
        catch (KpiScheduleClientException)
        {
            Interlocked.Increment(ref clientExceptionsCount);
        }
        catch (Exception)
        {
            Interlocked.Increment(ref unhandledExceptionsCount);
            logger.Fatal("Caught an unhandled exception when trying to parse scheduleId {scheduleId}", teacherScheduleId);
        }
    });

    var options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
    };

    logger.Information("Total exceptions caught during parsing: {parserExceptionsCount} during parsing, {clientExceptionsCount} from clients, {unhandledExceptionsCount} unhandled", parserExceptionsCount, clientExceptionsCount, unhandledExceptionsCount);
    logger.Information("Parsed a total of {schedulesCount} schedules, writing them to teacher-schedules.json", teacherSchedules.Count);

    var schedulesJson = JsonSerializer.Serialize(teacherSchedules, options);

    File.WriteAllText("teacher-schedules.json", schedulesJson);
}