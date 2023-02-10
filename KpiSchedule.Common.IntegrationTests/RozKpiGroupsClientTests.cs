using FluentAssertions;
using KpiSchedule.Common.Clients;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    //[Ignore("roz.kpi.ua API is not responding")]
    internal class RozKpiGroupsClientTests
    {
        private IServiceProvider serviceProvider;
        private RozKpiApiClient client => serviceProvider.GetRequiredService<RozKpiApiClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddMockRozKpiApiClient(config)
                .BuildServiceProvider();
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
        public async Task GetTeachers_WithValidGroupRequest_ShouldReturnTeachersList()
        {
            var teacherNamePrefix = "А";

            var teachers = await client.GetTeachers(teacherNamePrefix);

            Assert.IsNotEmpty(teachers.Data);
            Assert.True(teachers.Data.All(g => g.StartsWith(teacherNamePrefix)));
        }

        [Test]
        public async Task GetGroupSelectionPage_ShouldReturnGroupSelectionPage()
        {
            var page = await client.GetGroupSelectionPage();

            page.DocumentNode.OuterHtml.Should().Contain("Розклад занять");
        }

        [Test]
        public async Task GetGroupSchedulePage_ShouldReturnGroupSchedulePage()
        {
            var groupName = "ІТ-04";
            var page = await client.GetGroupSchedulePage(groupName);

            page.DocumentNode.OuterHtml.Should().Contain("Розклад занять для " + groupName);
        }
    }
}
