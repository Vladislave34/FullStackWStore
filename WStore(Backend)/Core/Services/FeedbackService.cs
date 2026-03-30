using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Feedback;
using Core.Models.ProductVariantImage;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class FeedbackService(AppStoreContext context,
    IRedisService redisService,
    IMapper mapper,
    IAuthService authService,
    IEntityImageService<FeedbackImageEntity,
            FeedbackImageAddModel, 
            FeedbackImageItemModel>
        feedbackImageService
        ): IFeedbackService
{
    public async Task AddFeedback(FeedbackAddUpdateModel model)
    {
        var entity = mapper.Map<FeedbackEntity>(model);
        entity.UserId = await authService.GetUserIdAsync();
        await context.Feedbacks.AddAsync(entity);
        await context.SaveChangesAsync();
        
        if (model.Images.Any())
        {
            await feedbackImageService.AddAllImages(entity.Id, model.Images.Select(f=>new FeedbackImageAddModel(){file = f}).ToList());
        }
        var feedback =  mapper.Map<FeedbackItemModel>(entity);
        await redisService.SetAsync($"feedback:{feedback.Id}", feedback, TimeSpan.FromMinutes(10));
        
    }

    public async Task UpdateFeedback(Guid id, FeedbackAddUpdateModel model)
    {
        var entity = await context.Feedbacks.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Feedback not found");
        }
        mapper.Map(model, entity);
        await feedbackImageService.UpdateAllImages(id,
            model.Images.Select(f => new FeedbackImageAddModel() { file = f }).ToList());
        await context.SaveChangesAsync();
        var feedback = mapper.Map<FeedbackItemModel>(entity);
        await redisService.SetAsync($"feedback:{id}", feedback, TimeSpan.FromMinutes(10));
        
        
    }

    public async Task RemoveFeedback(Guid id)
    {
        var entity = await context.Feedbacks.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Feedback not found");
        }
        entity.IsDeleted = true;
        await feedbackImageService.DeleteAllImages(id);
        await context.SaveChangesAsync();

    }

    public async Task RemoveAllFeedbacks()
    {
        var feedbacks = await context.Feedbacks.Where(x=>!x.IsDeleted).ToListAsync();
        foreach (var feedback in feedbacks)
        {
            feedback.IsDeleted = true;
            await feedbackImageService.DeleteAllImages(feedback.Id);
            await redisService.RemoveAsync($"feedback:{feedback.Id}");
        }
        await context.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<FeedbackItemModel>> GetAllFeedbacksByProductId(Guid productId)
    {
        
        
        
        var feedbacks = await context.Feedbacks.
            Where(x=>x.ProductId == productId)
            .ProjectTo<FeedbackItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        
        return feedbacks;
            
    }

    public async Task<FeedbackItemModel> GetFeedbackById(Guid id)
    {
        string key = $"feedback:{id}";
        var cache = await redisService.GetAsync<FeedbackItemModel>(key);
        if(cache != null){return cache;}
        
        var entity = await context.Feedbacks.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);

        var feedback = mapper.Map<FeedbackItemModel>(entity);
        await redisService.SetAsync(key, feedback, TimeSpan.FromMinutes(10));
        return feedback;


    }
}