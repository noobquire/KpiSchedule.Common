using AutoMapper;
using KpiSchedule.Common.Entities.RozKpi;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Mappers
{
    public class RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile : Profile
    {
        public RozKpiApiGroupSchedule_GroupScheduleEntity_MapperProfile()
        {
            CreateMap<RozKpiApiGroupScheduleDay, GroupScheduleDayEntity>().ReverseMap();
            CreateMap<RozKpiApiGroupPair, GroupPairEntity>();
            CreateMap<GroupPairEntity, RozKpiApiGroupPair>()
                .ForMember(d => d.StartTime, x => x.MapFrom(s => TimeOnly.Parse(s.startTime)))
                .ForMember(d => d.EndTime, x => x.MapFrom(s => TimeOnly.Parse(s.endTime)));
            CreateMap<RozKpiApiTeacher, TeacherEntity>().ReverseMap();
            CreateMap<RozKpiApiSubject, SubjectEntity>().ReverseMap();
            CreateMap<RozKpiApiGroupSchedule, GroupScheduleEntity>();
            CreateMap<GroupScheduleEntity, RozKpiApiGroupSchedule>();
        }
    }
}
