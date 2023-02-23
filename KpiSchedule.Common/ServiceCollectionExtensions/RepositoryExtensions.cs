using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddDynamoDbSchedulesRepository<TRepository, TSchedule, TDay, TPair>(this IServiceCollection services, IConfiguration config) 
            where TRepository : class, ISchedulesRepository<TSchedule, TDay, TPair>
            where TSchedule : BaseScheduleEntity<TDay, TPair>
            where TDay : BaseScheduleDayEntity<TPair>
            where TPair : BaseSchedulePairEntity
        {
            services.AddDefaultAWSOptions(config.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<TRepository>();

            return services;
        }

        public static IServiceCollection AddDynamoDbSchedulesRepository<TRepositoryInterface, TRepository, TSchedule, TDay, TPair>(this IServiceCollection services, IConfiguration config)
            where TRepositoryInterface : class
            where TRepository : class, ISchedulesRepository<TSchedule, TDay, TPair>, TRepositoryInterface
            where TSchedule : BaseScheduleEntity<TDay, TPair>
            where TDay : BaseScheduleDayEntity<TPair>
            where TPair : BaseSchedulePairEntity
        {
            services.AddDefaultAWSOptions(config.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<TRepositoryInterface, TRepository>();

            return services;
        }
    }
}
