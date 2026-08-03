using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class PaymentEntity : BaseEntity<Guid>
{
    [Required]
    public string Number { get; set; }
    [Required]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Формат має бути MM/YY")]
    
    public string Date { get; set; }
    
    [Required]
    public string CVV { get; set; }
    [Required]
    public string OwnerName { get; set; }
    [Required]
    public string PaymentSystem { get; set; }
    
    [ForeignKey(nameof(UserEntity))]
    public Guid? UserId { get; set; }
    public UserEntity? User { get; set; }
    
    
    public ICollection<OrderEntity> Orders { get; set; }
}