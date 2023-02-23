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
            CreateMap<RozKpiApiGroupPair, GroupSchedulePairEntity>();
            CreateMap<GroupSchedulePairEntity, RozKpiApiGroupPair>()
                .ForMember(d => d.StartTime, x => x.MapFrom(s => TimeOnly.Parse(s.StartTime)))
                .ForMember(d => d.EndTime, x => x.MapFrom(s => TimeOnly.Parse(s.EndTime)));
            CreateMap<RozKpiApiTeacher, TeacherEntity>().ReverseMap();
            CreateMap<RozKpiApiSubject, SubjectEntity>().ReverseMap();
            CreateMap<RozKpiApiGroupSchedule, GroupScheduleEntity>();
            CreateMap<GroupScheduleEntity, RozKpiApiGroupSchedule>();
        }
    }
}
