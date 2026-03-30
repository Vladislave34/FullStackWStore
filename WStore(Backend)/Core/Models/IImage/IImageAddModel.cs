using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IImageAddModel
{
    IFormFile file { get; set; }
}