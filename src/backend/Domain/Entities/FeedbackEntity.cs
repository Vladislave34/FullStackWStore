using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Domain.Entities;

public class FeedbackEntity : BaseEntity<Guid>
{
    [Required]public string Text { get; set; }
    [Required]public int Rating { get; set; }
    [ForeignKey(nameof(UserEntity))] public Guid UserId { get; set; }
    public  UserEntity User { get; set; }
    [ForeignKey(nameof(ProductEntity))] 
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }
    public ICollection<FeedbackImageEntity> Images { get; set; } = new List<FeedbackImageEntity>();

}