using AutoMapper;
using KpiSchedule.Common.Entities.RozKpi;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Mappers
{
    public class RozKpiApiTeacherSchedule_TeacherScheduleEntity_MapperProfile : Profile
    {
        public RozKpiApiTeacherSchedule_TeacherScheduleEntity_MapperProfile()
        {
            CreateMap<RozKpiApiTeacherScheduleDay, TeacherScheduleDayEntity>().ReverseMap();
            CreateMap<RozKpiApiTeacherPair, TeacherPairEntity>();
            CreateMap<TeacherPairEntity, RozKpiApiTeacherPair>()
                .ForMember(d => d.StartTime, x => x.MapFrom(s => TimeOnly.Parse(s.startTime)))
                .ForMember(d => d.EndTime, x => x.MapFrom(s => TimeOnly.Parse(s.endTime)));
            CreateMap<RozKpiApiSubject, SubjectEntity>().ReverseMap();
            CreateMap<RozKpiApiTeacherSchedule, TeacherScheduleEntity>();
            CreateMap<TeacherScheduleEntity, RozKpiApiTeacherSchedule>();
        }
    }
}
