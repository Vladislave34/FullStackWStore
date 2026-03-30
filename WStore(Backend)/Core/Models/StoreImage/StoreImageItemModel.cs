using Core.Interfaces;

namespace Core.Models.StoreImage;

public class StoreImageItemModel: IImageItemModel
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public string Store { get; set; }
}