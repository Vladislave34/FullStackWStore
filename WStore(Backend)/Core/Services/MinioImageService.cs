using Amazon.S3;
using Amazon.S3.Model;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Core.Services;

public class MinioImageService(IAmazonS3 s3Client, IConfiguration  config) : IMinioImageService
{
    private readonly string _bucketName = config["MinIO:BucketName"]!;
    private readonly string _endpoint = config["MinIO:Endpoint"]!;


    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var key = $"images/{Guid.NewGuid()}.webp"; 

        using var inputStream = file.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);
    
        image.Mutate(img => img.Resize(new ResizeOptions
        {
            Size = new Size(600, 600),
            Mode = ResizeMode.Max,
        }));

        using var outputStream = new MemoryStream();
        await image.SaveAsWebpAsync(outputStream); 
        outputStream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = outputStream,
            ContentType = "image/webp" // ← webp
        };

        await s3Client.PutObjectAsync(request);
        return $"{_endpoint}/{_bucketName}/{key}";
    }

    public async Task<string> UploadImageFromUrlAsync(string url)
    {
        using var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(url);

        using var image = Image.Load(bytes);
        image.Mutate(img => img.Resize(new ResizeOptions
        {
            Size = new Size(600, 600),
            Mode = ResizeMode.Max,
        }));

        using var outputStream = new MemoryStream();
        await image.SaveAsWebpAsync(outputStream); // ← webp
        outputStream.Position = 0;

        var key = $"images/{Guid.NewGuid()}.webp"; 

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = outputStream,
            ContentType = "image/webp" 
        };

        await s3Client.PutObjectAsync(request);
        return $"{_endpoint}/{_bucketName}/{key}";
    }
    public async Task DeleteImageAsync(string path)
    {
        var key = path.Replace($"{_endpoint}/{_bucketName}/", "");

        await s3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        });
    }
    public async Task<string> UpdateImageAsync(string oldPath, IFormFile newFile)
    {
        await DeleteImageAsync(oldPath);
        return await UploadImageAsync(newFile);
    }
    
}