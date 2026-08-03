namespace Core.Models.Product.Statistics;

public sealed record StatisticIncomeByCategoryRequest
{
    
    
        public  DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string Category { get; init; }
        public string CategoryUk { get; init; }
        public  double Income { get; init; }
    
}