using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ColorEntity : BaseEntity<Guid>
{
    [Required] public string Name { get; set; }
    [Required] public string Hex { get; set; }

    public ICollection<ProductVariantEntity> ProductVariants { get; set; } = new List<ProductVariantEntity>();
}
