namespace Core.Models.Product.Statistics;

public sealed record StatisticIncomeRequest
{
    public  DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public  double Income { get; init; }
}