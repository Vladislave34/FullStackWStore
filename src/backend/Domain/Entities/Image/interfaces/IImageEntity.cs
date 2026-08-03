namespace Core.Interfaces;

public interface IImageEntity
{
    Guid Id { get; set; }
    Guid ParentId { get; set; }
    string Path { get; set; }
    bool IsDeleted { get; set; }
}