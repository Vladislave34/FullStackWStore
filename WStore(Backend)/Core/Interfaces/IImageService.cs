using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task<string> UploadImageByLinkAsync(string link);
    Task DeleteImageAsync(string fileName);
    Task<string> UpdateImageAsync(IFormFile file,  string fileName);
}