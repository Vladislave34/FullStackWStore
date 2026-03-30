using Core.Models.Feedback;

namespace Core.Interfaces;

public interface IFeedbackService
{
    Task AddFeedback(FeedbackAddUpdateModel model);
    Task UpdateFeedback(Guid id, FeedbackAddUpdateModel model);
    Task RemoveFeedback(Guid id);
    
    Task RemoveAllFeedbacks();
    
    Task<IEnumerable<FeedbackItemModel>> GetAllFeedbacksByProductId(Guid productId);
    
    Task<FeedbackItemModel> GetFeedbackById(Guid id);
}