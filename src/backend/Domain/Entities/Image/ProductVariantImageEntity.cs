using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces;
using Domain.Entities.Identity;

namespace Domain.Entities;
[Table("ProductImages")]
public class ProductVariantImageEntity : BaseEntity<Guid>, IImageEntity
{
    public string Path { get; set; }

    public Guid ProductVariantId { get; set; }
    public ProductVariantEntity ProductVariant { get; set; }

    public Guid ParentId
    {
        get => ProductVariantId;
        set => ProductVariantId = value;
    }
}
    