using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OMNE.Data.Services;

public class DbInitializerService<T>(IServiceScopeFactory scopeFactory) : IHostedService where T : DbContext
{
    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
        => MigrateAsync(cancellationToken);

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private async Task MigrateAsync(CancellationToken cancellation)
    {
        using var scope = scopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<T>();
        await MigrateAsync(context, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    private static Task MigrateAsync(DbContext context, CancellationToken cancellation)
    {
        var migrator = context.Database.GetInfrastructure().GetService<IMigrator>();
        return migrator?.HasPendingModelChanges() switch
        {
            false => migrator.MigrateAsync(cancellationToken: cancellation),
            true => EnsureCreatedAsync(context, cancellation),
            _ => Task.CompletedTask,
        };
    }

    private static async Task EnsureCreatedAsync(DbContext context, CancellationToken cancellation)
    {
        var provider = context.Database.GetInfrastructure();
        var creator = provider.GetRequiredService<IDatabaseCreator>();

        if (creator is IRelationalDatabaseCreator rdbms)
            await EnsureCreatedAsync(rdbms, cancellation);
        else
            await creator.EnsureCreatedAsync(cancellation);
    }

    private static async Task EnsureCreatedAsync(IRelationalDatabaseCreator rdbms, CancellationToken cancellation)
    {
        var hasTablesAsync = false;
        try
        {
            if (await rdbms.ExistsAsync(cancellation))
                hasTablesAsync = await rdbms.HasTablesAsync(cancellation);
            else
                await rdbms.CreateAsync(cancellation);
        }
        catch (DbException e)
        {
            Console.WriteLine("Unable to determine if tables exist, assuming no tables yet.");
            Console.WriteLine(e);
        }

        if (!hasTablesAsync)
            await rdbms.CreateTablesAsync(cancellation);
    }
}
