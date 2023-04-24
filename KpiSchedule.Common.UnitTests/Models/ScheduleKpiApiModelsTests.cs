using FluentAssertions;
using KpiSchedule.Common.Entities.Group;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;
using KpiSchedule.Common.Models.ScheduleKpiApi.Group;
using KpiSchedule.Common.Models.ScheduleKpiApi.Teacher;
using System.Text.Json;

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
                    Id = "f4382a6b-269e-4cb7-86dd-8120a731b9df",
                    GroupName = "МВ-01",
                    Faculty = "ММІ"
                },
                new ScheduleKpiApiGroup()
                {
                    Id = "39ce3b8d-ae35-4b36-90a2-2bead39b1ecc",
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
        public void DeserializeLecturersResponse_Success()
        {
            var json = "{\"paging\":{\"pageCount\":1,\"totalItemCount\":2454,\"pageNumber\":1,\"pageSize\":2454,\"hasPreviousPage\":false,\"hasNextPage\":false,\"isFirstPage\":true,\"isLastPage\":true,\"firstItemOnPage\":1,\"lastItemOnPage\":2454},\"data\":[{\"id\":\"2d84f91e-b6d2-427d-8d81-84a2d64c998c\",\"name\":\"Лещенко Борис Юхимович\"}]}";

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ScheduleKpiApiTeachersResponse>(json, deserializationOptions);
        }

        [Test]
        public void DeserializeGroupPairEntity_Success()
        {
            var json = "{\"endTime\":\"10:05:00\",\"isOnline\":true,\"pairNumber\":1,\"rooms\":[\"-18\"],\"startTime\":\"08:30:00\",\"subject\":{\"subjectFullName\":\"Групова динаміка та комунікації\",\"subjectName\":\"Групова динаміка та комунікації\"},\"teachers\":[{\"scheduleId\":\"8ec3f3d5-cd4c-4eaa-a834-ff56934b60b3\",\"fullName\":\"посада Ясенова Анна Вадимівна\",\"shortName\":\"пос. Ясенова А. В.\"}],\"type\":\"prac\"}";

            var result = JsonSerializer.Deserialize<GroupSchedulePairEntity>(json);
        }
    }
}
