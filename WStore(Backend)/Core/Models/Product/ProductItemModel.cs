using Domain.Entities;

namespace Core.Models.Product;

public class ProductItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Store { get; set; }
}