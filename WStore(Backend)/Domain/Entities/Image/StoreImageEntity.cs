using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces;

namespace Domain.Entities;

public class StoreImageEntity : BaseEntity<Guid>, IImageEntity
{
    public string Path { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity Store { get; set; }

    public Guid ParentId
    {
        get => StoreId;
        set => StoreId = value;
    }
}