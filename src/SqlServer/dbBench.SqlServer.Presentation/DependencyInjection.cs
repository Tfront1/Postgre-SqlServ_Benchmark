using dbBench.Application.Services.DataGenerators;
using dbBench.Infrastructure.Database.Contexts;
using dbBench.Infrastructure.Services.DataGenerators;
using Microsoft.EntityFrameworkCore;

namespace dbBench.SqlServer.Presentation;

public static class DependencyInjection
{
    public static async Task<IServiceProvider> PrepareDbAsync(
        this IServiceProvider services)

    {
        await using var scope = services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<sqlServerBenchContext>();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
            await context.Database.MigrateAsync();

        return services;
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserGenerator, UserGenerator>();
    }
}

