namespace Core.Models.Product.Statistics;

public class StatisticIncomeRequest
{
    public  DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public  double Income { get; set; }
}