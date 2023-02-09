using FluentAssertions;
using KpiSchedule.Common.Models.ScheduleKpiApi;
using KpiSchedule.Common.Models.ScheduleKpiApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KpiSchedule.Common.UnitTests.Models
{
    public class ScheduleKpiApiModelsTests
    {
        [Test]
        public void DeserializePagingInfo_Success()
        {
            var json = "{\"pageCount\":1,\"totalItemCount\":1346,\"pageNumber\":1,\"pageSize\":1346,\"hasPreviousPage\":false,\"hasNextPage\":false,\"isFirstPage\":true,\"isLastPage\":true,\"firstItemOnPage\":1,\"lastItemOnPage\":1346}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiPaging>(json, deserializationOptions);
        }

        [Test]
        public void DeserializeGroup_Success()
        {
            var json = "{\"id\":\"f4382a6b-269e-4cb7-86dd-8120a731b9df\",\"name\":\"МВ-01\",\"faculty\":\"ММІ\"}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiGroup>(json, deserializationOptions);
        }

        [Test]
        public void DeserializeGroupsResponse_Success()
        {
            var json = "{\"paging\":{\"pageCount\":1,\"totalItemCount\":1346,\"pageNumber\":1,\"pageSize\":1346,\"hasPreviousPage\":false,\"hasNextPage\":false,\"isFirstPage\":true,\"isLastPage\":true,\"firstItemOnPage\":1,\"lastItemOnPage\":1346},\"data\":[{\"id\":\"f4382a6b-269e-4cb7-86dd-8120a731b9df\",\"name\":\"МВ-01\",\"faculty\":\"ММІ\"},{\"id\":\"39ce3b8d-ae35-4b36-90a2-2bead39b1ecc\",\"name\":\"МВ-01\",\"faculty\":\"ВПІ\"}]}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiGroupsResponse>(json, deserializationOptions);
        }

        [Test]
        public void SerializeDeserializeGroupsResponse_Equialent()
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

    }
}
