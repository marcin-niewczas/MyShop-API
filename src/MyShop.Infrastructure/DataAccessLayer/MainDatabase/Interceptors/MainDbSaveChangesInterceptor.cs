using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Interceptors;
internal sealed class MainDbSaveChangesInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var now = timeProvider.GetUtcNow();

        FillInsertedEntitesTimestamps(eventData, now);
        FillModifiedEntitesTimestamps(eventData, now);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow();

        FillInsertedEntitesTimestamps(eventData, now);
        FillModifiedEntitesTimestamps(eventData, now);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void FillInsertedEntitesTimestamps(DbContextEventData eventData, DateTimeOffset now)
    {
        var insertedEntries = eventData.Context?.ChangeTracker.Entries()
                               .Where(e => e.State == EntityState.Added)
                               .Select(e => e.Entity);

        if (insertedEntries is null || !insertedEntries.Any())
        {
            return;
        }

        foreach (var insertedEntry in insertedEntries)
        {
            if (insertedEntry is ITimestampEntity entity)
            {
                entity.CreatedAt = now;
            }
        }
    }

    private static void FillModifiedEntitesTimestamps(DbContextEventData eventData, DateTimeOffset now)
    {
        var modifiedEntries = eventData.Context?.ChangeTracker.Entries()
                   .Where(e => e.State == EntityState.Modified)
                   .Select(e => e.Entity);

        if (modifiedEntries is null || !modifiedEntries.Any())
        {
            return;
        }

        foreach (var modifiedEntry in modifiedEntries)
        {
            if (modifiedEntry is ITimestampEntity entity)
            {
                entity.UpdatedAt = now;
            }
        }
    }
}
