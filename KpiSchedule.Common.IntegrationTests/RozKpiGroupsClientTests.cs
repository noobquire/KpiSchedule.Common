using KpiSchedule.Common.Clients;
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
        private RozKpiApiClient client => serviceProvider.GetRequiredService<RozKpiApiClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddKpiClient<RozKpiApiClient>(config)
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
    }
}
