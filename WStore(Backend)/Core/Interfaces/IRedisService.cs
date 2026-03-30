namespace Core.Interfaces;

public interface IRedisService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T data, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
}