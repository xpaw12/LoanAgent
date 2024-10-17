using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace LoanAgent.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    private static EventId[] IgnoredCoreEvents => new[]
    {
            CoreEventId.QueryCompilationStarting,
            CoreEventId.QueryExecutionPlanned,
            CoreEventId.NavigationBaseIncluded,
            CoreEventId.ValueGenerated,
            CoreEventId.StateChanged
        };

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.ConfigureWarnings(b => b.Ignore(IgnoredCoreEvents));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}