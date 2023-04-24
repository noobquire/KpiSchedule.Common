using FluentAssertions;
using KpiSchedule.Common.Entities.Group;
using KpiSchedule.Common.Mappers;
using KpiSchedule.Common.Models.RozKpiApi.Group;
using System.Text.Json;

namespace KpiSchedule.Common.UnitTests.Mappers
{
    [TestFixture]
    public class GroupScheduleMapperTests
    {
        [Test]
        public void Map_RozKpiSchedule_ToEntity_Success()
        {
            var rozKpiScheduleJson = File.ReadAllText("TestData/testGroupSchedule.json");
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var rozKpiSchedule = JsonSerializer.Deserialize<RozKpiApiGroupSchedule>(rozKpiScheduleJson, options);

            var result = rozKpiSchedule.MapToEntity();

            result.ScheduleId.Should().Be(rozKpiSchedule.ScheduleId);
            result.GroupName.Should().Be(rozKpiSchedule.GroupName);
            result.FirstWeek.Count.Should().Be(rozKpiSchedule.FirstWeek.Count);
            result.FirstWeek[0].Pairs[0].Subject.SubjectName
                .Should().Be(rozKpiSchedule.FirstWeek[0].Pairs[0].Subject.SubjectName);
        }

        [Test]
        public void Map_GroupScheduleEntity_ToRozKpiSchedule_Success()
        {
            var groupScheduleEntityJson = File.ReadAllText("TestData/testGroupSchedule.json");
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var groupScheduleEntity = JsonSerializer.Deserialize<GroupScheduleEntity>(groupScheduleEntityJson, options);

            var result = groupScheduleEntity.MapToModel()!;

            result.ScheduleId.Should().Be(groupScheduleEntity.ScheduleId);
            result.GroupName.Should().Be(groupScheduleEntity.GroupName);
            result.FirstWeek.Count.Should().Be(groupScheduleEntity.FirstWeek.Count);
            result.FirstWeek[0].Pairs[0].Subject.SubjectName
                .Should().Be(groupScheduleEntity.FirstWeek[0].Pairs[0].Subject.SubjectName);
        }
    }
}
