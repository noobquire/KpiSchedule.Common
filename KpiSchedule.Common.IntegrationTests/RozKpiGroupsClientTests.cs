using FluentAssertions;
using KpiSchedule.Common.Clients;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    internal class RozKpiGroupsClientTests
    {
        private IServiceProvider serviceProvider;
        private RozKpiApiGroupsClient client => serviceProvider.GetRequiredService<RozKpiApiGroupsClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            bool mockRozKpiApiResponses = config.GetSection("MockRozKpiApiResponses").Get<bool>();
            var services = new ServiceCollection()
                .AddSerilogConsoleLogger(LogEventLevel.Information)
                .AddRozKpiParsers();

            if (mockRozKpiApiResponses)
            {
                services.AddMockRozKpiApiClients(config);
            }
            else
            {
                services
                    .AddKpiClient<RozKpiApiTeachersClient>(config)
                    .AddKpiClient<RozKpiApiGroupsClient>(config);
            }

            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task GetGroups_WithValidGroupRequest_ShouldReturnGroupsList()
        {
            var groupPrefix = "І";

            var groups = await client.GetGroups(groupPrefix);

            Assert.IsNotEmpty(groups.Data);
            Assert.True(groups.Data.All(g => g.StartsWith(groupPrefix)));
        }

        [Test]
        public async Task GetGroupSelectionPage_ShouldReturnGroupSelectionPage()
        {
            var page = await client.GetGroupSelectionPage();

            page.DocumentNode.OuterHtml.Should().Contain("Розклад занять");
        }

        [Test]
        public async Task GetGroupScheduleIds_ShouldReturnGroupScheduleId()
        {
            var groupName = "ІТ-04";
            var scheduleId = (await client.GetGroupScheduleIds(groupName)).First();

            var expectedId = new Guid("13623e82-3f89-4815-b475-df7f442832e6");
            scheduleId.Should().Be(expectedId);
        }

        [Test]
        public async Task GetGroupScheduleIds_ConflictingGroupNames_ShouldReturnAllScheduleGroupIds()
        {
            var groupName = "БМ-01";
            var scheduleIds = (await client.GetGroupScheduleIds(groupName));

            var expectedIds = new[] 
            {
                new Guid("660d6d99-5eef-4aaf-9a50-66171822b53a"),
                new Guid("613065b1-78cb-4a79-8823-f2cfda862c0a")
            };

            scheduleIds.Should().BeEquivalentTo(expectedIds);
        }

        [Test]
        public async Task GetGroupSchedulePage_ShouldReturnGroupSchedulePage()
        {
            var groupId = new Guid("13623e82-3f89-4815-b475-df7f442832e6");
            var groupName = "ІТ-04";
            var page = await client.GetSchedulePage(groupId, RozKpiApiScheduleType.GroupSchedule);

            page.DocumentNode.OuterHtml.Should().Contain($"Розклад занять для {groupName}");
        }

        [Test]
        public async Task GetParsedGroupSchedule_ShouldReturnGroupSchedule()
        {
            var groupId = new Guid("13623e82-3f89-4815-b475-df7f442832e6");
            var groupName = "ІТ-04";
            var schedule = await client.GetGroupSchedule(groupId);

            schedule.GroupName.Should().Be(groupName);
        }
    }
}
