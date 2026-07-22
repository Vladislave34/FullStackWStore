namespace Core.Models.Product.Statistics;

public class StatisticIncomeByCategoryRequest
{
    
    
        public  DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; }
        public string CategoryUk { get; set; }
        public  double Income { get; set; }
    
}