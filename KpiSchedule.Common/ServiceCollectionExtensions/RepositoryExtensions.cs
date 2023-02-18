using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddDynamoDbSchedulesRepository<TRepository, TSchedule>(this IServiceCollection services, IConfiguration config) where TRepository : BaseDynamoDbSchedulesRepository<TSchedule>
        {
            services.AddDefaultAWSOptions(config.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<TRepository>();

            return services;
        }
    }
}
