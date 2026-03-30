using System.Net.Mime;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
namespace Core.Services;

public class ImageService(IConfiguration configuration)  : IImageService
{
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        try
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileName = System.IO.Path.GetRandomFileName() + ".webp";
            var bytes = ms.ToArray();
            using var image = Image.Load(bytes);
            image.Mutate(img =>
            {
                img.Resize(new ResizeOptions
                {
                    Size = new Size(600, 600),
                    Mode = ResizeMode.Max,
                });
            });
            var dirImageName = configuration["DirImageName"] ?? "images";
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), dirImageName, fileName);
            await image.SaveAsync(path, new WebpEncoder());
            return fileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return String.Empty;
        }
        

    }

    public async Task<string> UploadImageByLinkAsync(string link)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(link);
                var fileName = Path.GetRandomFileName() + ".webp";
                using var image = Image.Load(bytes);
                image.Mutate(img =>
                {
                    img.Resize(new ResizeOptions
                    {
                        Size = new Size(600, 600),
                        Mode = ResizeMode.Max,
                    });
                });
                var dirImageName = configuration["DirImageName"] ?? "images";
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), dirImageName, fileName);
                await image.SaveAsync(path, new WebpEncoder());
                return fileName;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return String.Empty;
        }
    }

    public async Task DeleteImageAsync(string fileName)
    {
        try
        {
            var dirImageName = configuration["DirImageName"];
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), dirImageName, fileName);
            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        
    }

    public async Task<string> UpdateImageAsync(IFormFile file,  string fileName)
    {
        if (file == null || file.Length == 0)
            return fileName ?? string.Empty;
        string newFileName = await UploadImageAsync(file);
        if (string.IsNullOrEmpty(newFileName))
            return fileName ?? string.Empty;

        
        if (!string.IsNullOrEmpty(fileName))
            await DeleteImageAsync(fileName);

        return newFileName;

    }
}