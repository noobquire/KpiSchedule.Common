using FluentAssertions;
using KpiSchedule.Common.Clients.RozKpiApi;
using KpiSchedule.Common.Models.ScheduleKpiApi;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    internal class ScheduleKpiGroupsClientTests
    {
        private IServiceProvider serviceProvider;
        private ScheduleKpiGroupsClient client => serviceProvider.GetRequiredService<ScheduleKpiGroupsClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddScheduleKpiGroupsClient(config)
                .BuildServiceProvider();
        }

        [Test]
        public void DeserializePagingInfo()
        {
            var json = "{\"pageCount\":1,\"totalItemCount\":1346,\"pageNumber\":1,\"pageSize\":1346,\"hasPreviousPage\":false,\"hasNextPage\":false,\"isFirstPage\":true,\"isLastPage\":true,\"firstItemOnPage\":1,\"lastItemOnPage\":1346}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiPaging>(json, deserializationOptions);
        }

        [Test]
        public void DeserializeGroup()
        {
            var json = "{\"id\":\"f4382a6b-269e-4cb7-86dd-8120a731b9df\",\"name\":\"МВ-01\",\"faculty\":\"ММІ\"}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiGroup>(json, deserializationOptions);
        }

        [Test]
        public void DeserializeGroupsResponse()
        {
            var json = "{\"paging\":{\"pageCount\":1,\"totalItemCount\":1346,\"pageNumber\":1,\"pageSize\":1346,\"hasPreviousPage\":false,\"hasNextPage\":false,\"isFirstPage\":true,\"isLastPage\":true,\"firstItemOnPage\":1,\"lastItemOnPage\":1346},\"data\":[{\"id\":\"f4382a6b-269e-4cb7-86dd-8120a731b9df\",\"name\":\"МВ-01\",\"faculty\":\"ММІ\"},{\"id\":\"39ce3b8d-ae35-4b36-90a2-2bead39b1ecc\",\"name\":\"МВ-01\",\"faculty\":\"ВПІ\"}]}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiGroupsResponse>(json, deserializationOptions);
        }

        [Test]
        public void SerializeDeserializeGroupsResponse()
        {
            var response = new ScheduleKpiApiGroupsResponse();
            response.Paging = new ScheduleKpiApiPaging()
            {
                FirstItemOnPage = 1,
                LastItemOnPage = 2,
                IsLastPage = true,
                IsFirstPage = true,
                HasNextPage = false,
                HasPreviousPage = false,
                PageCount = 1,
                PageSize = 2,
                TotalItemCount = 2,
            };
            response.Data = new List<ScheduleKpiApiGroup>
            {
                new ScheduleKpiApiGroup()
                {
                    Id = new Guid("f4382a6b-269e-4cb7-86dd-8120a731b9df"),
                    GroupName = "МВ-01",
                    Faculty = "ММІ"
                },
                new ScheduleKpiApiGroup()
                {
                    Id = new Guid("39ce3b8d-ae35-4b36-90a2-2bead39b1ecc"),
                    GroupName = "МВ-01",
                    Faculty = "ВПІ"
                },
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(response, serializerOptions);

            var result = JsonSerializer.Deserialize<ScheduleKpiApiGroupsResponse>(json, serializerOptions);

            result.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task GetAllGroups_ShouldReturnGroupsList()
        {
            var groups = await client.GetAllGroups();

            Assert.IsNotEmpty(groups.Data);
        }
    }
}
