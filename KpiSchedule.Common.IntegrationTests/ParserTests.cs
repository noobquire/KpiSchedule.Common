using FluentAssertions;
using HtmlAgilityPack;
using KpiSchedule.Common.Parsers.GroupSchedulePage;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog.Events;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    public class ParserTests
    {
        private HtmlDocument groupScheduleDocument;
        private HtmlDocument conflictingGroupsListDocument;
        private IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var groupScheduleHtml = File.ReadAllText("RozKpiApiResponses/group-schedule-page.html");
            groupScheduleDocument = new HtmlDocument();
            groupScheduleDocument.LoadHtml(groupScheduleHtml);

            var conflictingGroupsListHtml = File.ReadAllText("RozKpiApiResponses/group-selection-name-conflict-page.html");
            conflictingGroupsListDocument = new HtmlDocument();
            conflictingGroupsListDocument.LoadHtml(conflictingGroupsListHtml);
        }

        [SetUp]
        public void Setup()
        {
            serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger(LogEventLevel.Verbose)
                .AddRozKpiParsers()
                .BuildServiceProvider();
        }

        [Test]
        public void GroupScheduleParser_ParsesGroupSchedule()
        {
            var parser = serviceProvider.GetService<GroupSchedulePageParser>();

            var schedule = parser.Parse(groupScheduleDocument.DocumentNode);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(
                    UnicodeRanges.All),
                WriteIndented = true
            };
            var scheduleJson = JsonSerializer.Serialize(schedule, options);

            schedule.Should().NotBeNull();
            File.WriteAllText("schedule.json", scheduleJson);
        }

        [Test]
        public void ConflictingGroupNamesParser_ParsesGroupList()
        {
            var parser = serviceProvider.GetService<ConflictingGroupNamesParser>();

            var groups = parser.Parse(conflictingGroupsListDocument.DocumentNode);

            groups.Count().Should().Be(2);
        }
    }
}
