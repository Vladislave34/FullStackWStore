using Core.Interfaces;
using Core.Models.Product.Statistics;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class StatisticService(AppStoreContext context, IRedisService redisService) : IStatisticService
{
    public async Task<ICollection<StatisticIncomeRequest>> GetStatistics(int days)
    {
        var to = DateTime.UtcNow.Date.AddDays(1); 
        var from = to.AddDays(-days);

        var orders = await context.Orders
            .Where(x => !x.IsDeleted
                        && x.OrderStatusId == new Guid("019f5637-4b62-71a1-bf11-7640263504c3")
                        && x.CreatedAt >= from
                        && x.CreatedAt < to)
            .ToListAsync();

        
        int intervalDays = 5;

        

        var result = new List<StatisticIncomeRequest>();
        var fromDate = DateOnly.FromDateTime(from);
        var toDate = DateOnly.FromDateTime(to.AddDays(-1));
        var grouped = orders
            .GroupBy(x => 
            {
                var daysSinceStart = (DateOnly.FromDateTime(x.CreatedAt).DayNumber - fromDate.DayNumber) / intervalDays;
                return fromDate.AddDays(daysSinceStart * intervalDays);
            })
            .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalPrice));

        for (var date = fromDate; date <= toDate; date = date.AddDays(intervalDays))
        {
            grouped.TryGetValue(date, out var income);

            result.Add(new StatisticIncomeRequest
            {
                StartDate = date.ToDateTime(TimeOnly.MinValue),
                EndDate = date.ToDateTime(TimeOnly.MaxValue),
                Income = (double)income
            });
        }

        return result;
    }

    public async Task<ICollection<StatisticIncomeByCategoryRequest>> GetStatisticsByCategory()
    {
        
        
        var completedStatusId = new Guid("019f5637-4b62-71a1-bf11-7640263504c3");

        var stats = await context.Orders
            .Where(o => !o.IsDeleted && o.OrderStatusId == completedStatusId)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ProductVariant.Product.CategoryId)
            .Select(g => new
            {
                CategoryId = g.Key,
                Income = g.Sum(i => i.Price * i.Quantity)
            })
            .ToListAsync();

        var categoryIds = stats.Select(s => s.CategoryId).ToList();

        var categories = await context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var result = stats
            .Select(s => new StatisticIncomeByCategoryRequest
            {
                Category = categories[s.CategoryId].Name,
                CategoryUk = categories[s.CategoryId].NameUk,
                Income = (double)s.Income
            })
            .ToList();

        return result;

    }
}