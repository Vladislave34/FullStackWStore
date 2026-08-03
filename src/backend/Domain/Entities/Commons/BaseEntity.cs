using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;



public abstract class BaseEntity<T> : IEntity<T>
{
    [Key]
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }= DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}