using AutoMapper;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Mappers
{
    public class RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile : Profile
    {
        public RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile()
        {
            CreateMap<RozKpiApiGroupScheduleDay, GroupScheduleDayEntity>().ReverseMap();
            CreateMap<RozKpiApiGroupPair, GroupSchedulePairEntity>()
                .ForMember(p => p.PairType, x => x.MapFrom(p => p.Type))
                .ReverseMap();
            CreateMap<GroupSchedulePairEntity, RozKpiApiGroupPair>()
                .ForMember(d => d.StartTime, x => x.MapFrom(s => TimeOnly.Parse(s.StartTime)))
                .ForMember(d => d.EndTime, x => x.MapFrom(s => TimeOnly.Parse(s.EndTime)));
            CreateMap<RozKpiApiTeacher, TeacherEntity>()
                .ForMember(t => t.TeacherName, x => x.MapFrom(t => t.ShortName))
                .ForMember(t => t.TeacherFullName, x => x.MapFrom(t => t.FullName))
                .ReverseMap();
            CreateMap<RozKpiApiSubject, SubjectEntity>().ReverseMap();
            CreateMap<RozKpiApiGroupSchedule, GroupScheduleEntity>();
            CreateMap<GroupScheduleEntity, RozKpiApiGroupSchedule>();
        }
    }
}
