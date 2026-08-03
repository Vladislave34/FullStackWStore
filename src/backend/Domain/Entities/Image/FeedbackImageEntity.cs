using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces;

namespace Domain.Entities;
[Table("FeedbackImages")]
public class FeedbackImageEntity : BaseEntity<Guid>, IImageEntity
{
    public string Path { get; set; }

    public Guid FeedbackId { get; set; }
    public FeedbackEntity Feedback { get; set; }

    public Guid ParentId
    {
        get => FeedbackId;
        set => FeedbackId = value;
    }
}