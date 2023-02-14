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

Console.OutputEncoding = Encoding.UTF8;
var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
var serviceProvider = new ServiceCollection()
    .AddSerilogConsoleLogger(LogEventLevel.Verbose)
    .AddRozKpiParsers()
    .AddKpiClient<RozKpiApiTeachersClient>(config)
    .AddKpiClient<RozKpiApiGroupsClient>(config)
    .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger>();

var rozKpiApiClient = serviceProvider.GetRequiredService<RozKpiApiGroupsClient>();

var ukrainianAlphabet = "абвгдеєжзиіїйклмнопрстуфхцчшщюя";

var groupNameTasks = ukrainianAlphabet.Select(async c => await rozKpiApiClient.GetGroups(c.ToString()));
var groupNames = new List<string>();
foreach (var groupNameTask in groupNameTasks)
{
    var groupNamesForPrefix = await groupNameTask;
    groupNames.AddRange(groupNamesForPrefix.Data);
}

var groupScheduleIdTasks = groupNames.Select(async groupName =>
{
    try
    {
        return (await rozKpiApiClient.GetGroupScheduleIds(groupName)).First();
    }
    catch (KpiScheduleClientGroupNotFoundException)
    {
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
var groupScheduleTasks = groupScheduleIds.Select(async id =>
{
    try
    {
        var schedule = await rozKpiApiClient.GetGroupSchedule(id);
        return schedule;
    }
    catch (KpiScheduleParserException)
    {
        return null;
    }
    catch (KpiScheduleClientException)
    {
        return null;
    }
    catch (Exception)
    {
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

logger.Information("Parsed a total of {schedulesCount} schedules, writing them to schedules.json", groupSchedules.Count);
var schedulesJson = JsonSerializer.Serialize(groupSchedules, options);

File.WriteAllText("schedules.json", schedulesJson);