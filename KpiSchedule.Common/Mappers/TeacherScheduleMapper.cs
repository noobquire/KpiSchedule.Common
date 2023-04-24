using KpiSchedule.Common.Entities.Group;
using KpiSchedule.Common.Entities.Teacher;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;
using KpiSchedule.Common.Parsers;
using KpiSchedule.Common.Utils;

namespace KpiSchedule.Common.Mappers
{
    public static class TeacherScheduleMapper
    {
        public static TeacherScheduleEntity MapToEntity(this RozKpiApiTeacherSchedule model)
        {
            var entity = new TeacherScheduleEntity
            {
                TeacherName = model.TeacherName,
                ScheduleId = model.ScheduleId,
                FirstWeek = model.FirstWeek.Select(d => d.MapToEntity()).ToList(),
                SecondWeek = model.SecondWeek.Select(d => d.MapToEntity()).ToList()
            };
            return entity;
        }

        public static TeacherScheduleDayEntity MapToEntity(this RozKpiApiTeacherScheduleDay model)
        {
            var entity = new TeacherScheduleDayEntity
            {
                DayNumber = model.DayNumber,
                Pairs = model.Pairs.Select(p => p.MapToEntity()).ToList()
            };
            return entity;
        }

        public static TeacherSchedulePairEntity MapToEntity(this RozKpiApiTeacherPair model)
        {
            var entity = new TeacherSchedulePairEntity
            {
                PairNumber = model.PairNumber,
                StartTime = model.StartTime.ToString("t"),
                EndTime = model.EndTime.ToString("t"),
                PairType = model.Type.ToEnumString(),
                IsOnline = model.IsOnline,
                Subject = model.Subject.MapToEntity(),
                Rooms = model.Rooms.ToList(),
                Groups = model.GroupNames.Select(g =>
                new GroupEntity
                {
                    GroupName = g
                }).ToList()
            };
            return entity;
        }

        public static TeacherEntity MapToEntity(this RozKpiApiTeacher model)
        {
            var entity = new TeacherEntity
            {
                TeacherName = model.ShortName,
                TeacherFullName = model.FullName,
                ScheduleId = model.ScheduleId
            };
            return entity;
        }

        public static RozKpiApiTeacherSchedule MapToModel(this TeacherScheduleEntity entity)
        {
            var model = new RozKpiApiTeacherSchedule
            {
                TeacherName = entity.TeacherName,
                ScheduleId = entity.ScheduleId,
                FirstWeek = entity.FirstWeek.Select(d => d.MapToModel()).ToList(),
                SecondWeek = entity.SecondWeek.Select(d => d.MapToModel()).ToList()
            };
            return model;
        }

        public static RozKpiApiTeacherScheduleDay MapToModel(this TeacherScheduleDayEntity entity)
        {
            var model = new RozKpiApiTeacherScheduleDay
            {
                DayNumber = entity.DayNumber,
                Pairs = entity.Pairs.Select(p => p.MapToModel()).ToList()
            };
            return model;
        }

        public static RozKpiApiTeacherPair MapToModel(this TeacherSchedulePairEntity entity)
        {
            var model = new RozKpiApiTeacherPair
            {
                PairNumber = entity.PairNumber,
                StartTime = TimeOnly.Parse(entity.StartTime),
                EndTime = TimeOnly.Parse(entity.EndTime),
                Type = PairTypeParser.ParsePairType(entity.PairType),
                IsOnline = entity.IsOnline,
                Subject = entity.Subject.MapToModel(),
                Rooms = entity.Rooms.ToList(),
                GroupNames = entity.Groups.Select(g => g.GroupName).ToList()
            };
            return model;
        }
    }
}
