﻿using KpiSchedule.Common.Clients;
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
using KpiSchedule.Common.Entities.RozKpi;
using KpiSchedule.Common.Repositories;

Console.OutputEncoding = Encoding.UTF8;
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var serviceProvider = new ServiceCollection()
    .AddSerilogConsoleLogger(LogEventLevel.Verbose)
    .AddRozKpiParsers()
    .AddKpiClient<RozKpiApiTeachersClient>(config)
    .AddKpiClient<RozKpiApiGroupsClient>(config)
    .AddAutoMapper(typeof(RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile))
    .AddDynamoDbSchedulesRepository<RozKpiGroupSchedulesRepository, GroupScheduleEntity>(config)
    .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger>();
var mapper = serviceProvider.GetService<IMapper>();
var repository = serviceProvider.GetService<RozKpiGroupSchedulesRepository>();
var rozKpiApiClient = serviceProvider.GetRequiredService<RozKpiApiGroupsClient>();

var ukrainianAlphabet = new[] { "ІТ" };//"абвгдеєжзиіїйклмнопрстуфхцчшщюя";

var groupNameTasks = ukrainianAlphabet.Select(async c => await rozKpiApiClient.GetGroups(c.ToString()));
var groupNames = new List<string>();
foreach (var groupNameTask in groupNameTasks)
{
    var groupNamesForPrefix = await groupNameTask;
    groupNames.AddRange(groupNamesForPrefix.Data);
}

logger!.Information("Got {groupNamesCount} group names from roz.kpi.ua", groupNames.Count);

int schedulesNotFoundCount = 0;
var groupScheduleIdTasks = groupNames.Select(async groupName =>
{
    try
    {
        return (await rozKpiApiClient.GetGroupScheduleIds(groupName)).First();
    }
    catch (KpiScheduleClientGroupNotFoundException)
    {
        schedulesNotFoundCount++;
        return Guid.Empty;
    }
});

var groupScheduleIds = new List<Guid>();
foreach (var groupScheduleIdTask in groupScheduleIdTasks)
{
    var id = await groupScheduleIdTask;
    if (id != Guid.Empty)
    {
        groupScheduleIds.Add(id);
    }
}

logger.Information("Got {scheduleIdsCount} scheduleIds from roz.kpi.ua, {schedulesNotFoundCount} scheduleIds were not found", groupScheduleIds.Count, schedulesNotFoundCount);

int parserExceptionsCount = 0, clientExceptionsCount = 0, unhandledExceptionsCount = 0;
var groupScheduleTasks = groupScheduleIds.Select(async id =>
{
    try
    {
        var schedule = await rozKpiApiClient.GetGroupSchedule(id);
        return schedule;
    }
    catch (KpiScheduleParserException)
    {
        parserExceptionsCount++;
        return null;
    }
    catch (KpiScheduleClientException)
    {
        clientExceptionsCount++;
        return null;
    }
    catch (Exception)
    {
        unhandledExceptionsCount++;
        logger.Fatal("Caught an unhandled exception when trying to parse scheduleId {scheduleId}", id);
        return null;
    }
});

var groupSchedules = new List<RozKpiApiGroupSchedule>();

foreach (var groupScheduleTask in groupScheduleTasks)
{
    var groupSchedule = await groupScheduleTask;
    if (groupSchedule != null)
    {
        groupSchedules.Add(groupSchedule);
    }
}

var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

logger.Information("Total exceptions caught during parsing: {parserExceptionsCount} during parsing, {clientExceptionsCount} from clients, {unhandledExceptionsCount} unhandled", parserExceptionsCount, clientExceptionsCount, unhandledExceptionsCount);
logger.Information("Parsed a total of {schedulesCount} schedules, writing them to schedules.json", groupSchedules.Count);
var schedulesJson = JsonSerializer.Serialize(groupSchedules, options);

File.WriteAllText("schedules.json", schedulesJson);

/*
 * // Uncomment to write schedules to DynamoDb
var mappedSchedules = mapper!.Map<IEnumerable<GroupScheduleEntity>>(groupSchedules);
logger.Information("Writing {schedulesCount} schedules to DynamoDb", groupSchedules.Count);
await repository!.BatchPutSchedules(mappedSchedules);

var schedulesFromRepo = await repository.SearchScheduleId("ІТ");
var schedulesDict = schedulesFromRepo.ToDictionary(s => s.scheduleId, s => s.groupName);
var schedulesDictJson = JsonSerializer.Serialize(schedulesDict, options);

logger.Information("Found those schedules in DynamoDb: {schedules}", schedulesDictJson);
*/