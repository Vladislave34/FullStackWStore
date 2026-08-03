using Core.Models.Product.Statistics;

namespace Core.Interfaces;

public interface IStatisticService
{
    Task<ICollection<StatisticIncomeRequest>> GetStatistics(int days);
    Task<ICollection<StatisticIncomeByCategoryRequest>> GetStatisticsByCategory();
}