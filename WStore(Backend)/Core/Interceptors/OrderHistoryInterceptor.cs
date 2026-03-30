using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Interceptors;

public class OrderHistoryInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return result;

        // ToList() - щоб зафіксувати колекцію до модифікації
        var entries = context.ChangeTracker.Entries<OrderEntity>()
            .Where(e => e.State == EntityState.Added ||
                        (e.State == EntityState.Modified &&
                         e.Property(x => x.OrderStatusId).IsModified))
            .ToList(); // ← обов'язково ToList()

        var histories = entries.Select(entry => new OrderHistoryEntity
        {
            OrderId = entry.Entity.Id,
            StatusId = entry.Entity.OrderStatusId
        }).ToList();

        if (histories.Any())
            await context.Set<OrderHistoryEntity>().AddRangeAsync(histories, cancellationToken);

        return result;
    }
}