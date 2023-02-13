// See https://aka.ms/new-console-template for more information
using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    .AddKpiClient<RozKpiApiClient>(config)
    .BuildServiceProvider();

var rozKpiApiClient = serviceProvider.GetRequiredService<RozKpiApiClient>();

var groupNames = await rozKpiApiClient.GetGroups("ІТ");
var groupScheduleIdTasks = groupNames.Data.Select(async groupName =>
{
    return (await rozKpiApiClient.GetGroupScheduleIds(groupName)).First();
});

var groupScheduleIds = await Task.WhenAll(groupScheduleIdTasks);
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
});

var groupSchedules = (await Task.WhenAll(groupScheduleTasks)).Where(s => s != null);

var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var schedulesJson = JsonSerializer.Serialize(groupSchedules, options);

File.WriteAllText("schedules.json", schedulesJson);