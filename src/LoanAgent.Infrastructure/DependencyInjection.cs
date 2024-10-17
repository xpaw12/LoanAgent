using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Infrastructure.Data;
using LoanAgent.Infrastructure.Data.Repositories;
using LoanAgent.Infrastructure.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Simpl;

namespace LoanAgent.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseRelatedServices(configuration);

        services.AddHostedService<LoanConsumerService>();

        return services;
    }

    private static void AddDatabaseRelatedServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("_LoanAgentDbMigrationHistory", "dbo"));

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }
}
