using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using KpiSchedule.Common.Repositories;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    public class GroupScheduleRepositoryTests
    {
        private GroupSchedulesRepository repository;

        [SetUp]
        public void Setup()
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
            var context = new DynamoDBContext(client);
            repository = new GroupSchedulesRepository(context);
        }

        [Test]
        public async Task GetSchedule_ReturnsSchedule()
        {
            var schedule = await repository.GetScheduleById(new Guid("606c1db1-0c7f-45e1-a4a0-60a06ad72eea"));

            schedule.Should().NotBeNull();
        }
    }
}
