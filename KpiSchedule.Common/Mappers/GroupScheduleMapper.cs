using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Parsers;
using KpiSchedule.Common.Utils;

namespace KpiSchedule.Common.Mappers
{
    public static class GroupScheduleMapper
    {
        public static GroupScheduleEntity MapToEntity(this RozKpiApiGroupSchedule model)
        {
            var entity = new GroupScheduleEntity
            {
                GroupName = model.GroupName,
                ScheduleId = model.ScheduleId,
                FirstWeek = model.FirstWeek.Select(d => d.MapToEntity()).ToList(),
                SecondWeek = model.SecondWeek.Select(d => d.MapToEntity()).ToList()
            };
            return entity;
        }

        public static GroupScheduleDayEntity MapToEntity(this RozKpiApiGroupScheduleDay model)
        {
            var entity = new GroupScheduleDayEntity
            {
                DayNumber = model.DayNumber,
                Pairs = model.Pairs.Select(p => p.MapToEntity()).ToList()
            };
            return entity;
        }

        public static GroupSchedulePairEntity MapToEntity(this RozKpiApiGroupPair model)
        {
            var entity = new GroupSchedulePairEntity
            {
                PairNumber = model.PairNumber,
                StartTime = model.StartTime.ToString("t"),
                EndTime = model.EndTime.ToString("t"),
                PairType = model.Type.ToEnumString(),
                IsOnline = model.IsOnline,
                Subject = model.Subject.MapToEntity(),
                Rooms = model.Rooms.ToList(),
                Teachers = model.Teachers.Select(t => t.MapToEntity()).ToList()
            };
            return entity;
        }

        public static SubjectEntity MapToEntity(this RozKpiApiSubject model)
        {
            var entity = new SubjectEntity
            {
                SubjectName = model.SubjectName,
                SubjectFullName = model.SubjectFullName
            };
            return entity;
        }

        public static RozKpiApiGroupSchedule MapToModel(this GroupScheduleEntity entity)
        {
            var model = new RozKpiApiGroupSchedule
            {
                GroupName = entity.GroupName,
                ScheduleId = entity.ScheduleId,
                FirstWeek = entity.FirstWeek.Select(d => d.MapToModel()).ToList(),
                SecondWeek = entity.SecondWeek.Select(d => d.MapToModel()).ToList()
            };
            return model;
        }

        public static RozKpiApiGroupScheduleDay MapToModel(this GroupScheduleDayEntity entity)
        {
            var model = new RozKpiApiGroupScheduleDay
            {
                DayNumber = entity.DayNumber,
                Pairs = entity.Pairs.Select(p => p.MapToModel()).ToList()
            };
            return model;
        }

        public static RozKpiApiGroupPair MapToModel(this GroupSchedulePairEntity entity)
        {
            var model = new RozKpiApiGroupPair
            {
                PairNumber = entity.PairNumber,
                StartTime = TimeOnly.Parse(entity.StartTime),
                EndTime = TimeOnly.Parse(entity.EndTime),
                Type = PairTypeParser.ParsePairType(entity.PairType),
                IsOnline = entity.IsOnline,
                Subject = entity.Subject.MapToModel(),
                Rooms = entity.Rooms.ToList(),
                Teachers = entity.Teachers.Select(t => t.MapToModel()).ToList()
            };
            return model;
        }

        public static RozKpiApiSubject MapToModel(this SubjectEntity entity)
        {
            var model = new RozKpiApiSubject
            {
                SubjectName = entity.SubjectName,
                SubjectFullName = entity.SubjectFullName
            };
            return model;
        }

        public static RozKpiApiTeacher MapToModel(this TeacherEntity entity)
        {
            var model = new RozKpiApiTeacher
            {
                ShortName = entity.TeacherName,
                FullName = entity.TeacherFullName,
                ScheduleId = entity.ScheduleId
            };
            return model;
        }
    }
}
