namespace Domain.Entities;

public class GenderEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string NameUk { get; set; }
}