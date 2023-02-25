using AutoMapper;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Mappers
{
    public class RozKpiApiTeacherSchedule_TeacherScheduleEntity_MapperProfile : Profile
    {
        public RozKpiApiTeacherSchedule_TeacherScheduleEntity_MapperProfile()
        {
            CreateMap<RozKpiApiTeacherScheduleDay, TeacherScheduleDayEntity>().ReverseMap();
            CreateMap<RozKpiApiTeacherPair, TeacherSchedulePairEntity>()
                .ForMember(p => p.PairType, x => x.MapFrom(p => p.Type))
                .ForMember(p => p.Rooms, x => x.MapFrom(p => p.Rooms.ToList()))
                .ForMember(p => p.Groups, x => x.MapFrom(p => p.GroupNames.Select(g => new GroupEntity() { GroupName = g })))
                .ReverseMap();
            CreateMap<TeacherSchedulePairEntity, RozKpiApiTeacherPair>()
                .ForMember(d => d.StartTime, x => x.MapFrom(s => TimeOnly.Parse(s.StartTime)))
                .ForMember(d => d.EndTime, x => x.MapFrom(s => TimeOnly.Parse(s.EndTime)));
            CreateMap<RozKpiApiSubject, SubjectEntity>().ReverseMap();
            CreateMap<RozKpiApiTeacherSchedule, TeacherScheduleEntity>();
            CreateMap<TeacherScheduleEntity, RozKpiApiTeacherSchedule>();
        }
    }
}
