namespace Domain.Entities;

public class SaleEntity : BaseEntity<Guid>
{
    public int Percent { get; set; }
}