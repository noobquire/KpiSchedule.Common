using KpiSchedule.Common.Clients.RozKpiApi;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    [Ignore("roz.kpi.ua API is not responding")]
    internal class RozKpiGroupsClientTests
    {
        private IServiceProvider serviceProvider;
        private RozKpiGroupsClient client => serviceProvider.GetRequiredService<RozKpiGroupsClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddRozKpiGroupsClient(config)
                .BuildServiceProvider();
        }

        [Test]
        public async Task GetGroups_WithValidGroupRequest_ShouldReturnGroupsList()
        {
            var groupPrefix = "І";

            var groups = await client.GetGroups(groupPrefix);

            Assert.IsNotEmpty(groups);
            Assert.That(groups, Is.All.StartsWith(groupPrefix));
        }
    }
}
