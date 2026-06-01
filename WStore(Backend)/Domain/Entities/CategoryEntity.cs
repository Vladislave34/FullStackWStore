using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;


[Table("Categories")]
public class CategoryEntity : BaseEntity<Guid>
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string NameUk { get; set; } = null!;
    

    [Required] public string image { get; set; } = null!;
    
    public  ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}