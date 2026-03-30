using System.Text.Json;
using Core.Interfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Core.Services;

public class RedisService  : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(data);
        await _db.StringSetAsync(key, json, expiry, When.Always, CommandFlags.None);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}