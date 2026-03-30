namespace Core.Models.Store;

public class StoreItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public List<string> Images { get; set; } = null!;
}