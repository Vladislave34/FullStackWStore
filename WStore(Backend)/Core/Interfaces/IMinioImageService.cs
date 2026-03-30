using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IMinioImageService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string path);
    Task<string> UpdateImageAsync(string oldPath, IFormFile newFile);
    Task<string> UploadImageFromUrlAsync(string url);

}