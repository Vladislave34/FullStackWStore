using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Models.StoreImage;

public class StoreImageAddUpdateModel :  IImageAddModel
{
    public IFormFile file { get; set; }
    
}