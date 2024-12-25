using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameService.Utility;

public static class RedisExtensions
{
    public static async Task<T?> GetObject<T>(this IDistributedCache cache, string key,
        CancellationToken token = default)
    {
        var value = await cache.GetStringAsync(key, token);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public static async Task SetObject<T>(this IDistributedCache cache, string key, T value,
        DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        var json = JsonConvert.SerializeObject(value);
        await cache.SetStringAsync(key, json, options, token);
    }

    public static async Task<long> Enqueue<T>(this IDatabase db, string key, T value)
    {
        var json = JsonConvert.SerializeObject(value);
        var redisKey = new RedisKey(key);
        return await db.ListRightPushAsync(redisKey, new RedisValue(json));
    }

    public static async Task<long> EnqueueRange<T>(this IDatabase db, string key, T[] value)
    {
        var values = value.Select(v => new RedisValue(JsonConvert.SerializeObject(v))).ToArray();
        var redisKey = new RedisKey(key);
        return await db.ListRightPushAsync(redisKey, values);
    }

    public static async Task<RedisValue[]> Pop(this IDatabase db, string key, long count)
    {
        return await db.ListLeftPopAsync(new RedisKey(key), count);
    }

    public static async Task<T[]> ListRange<T>(this IDatabase db, string key, long start = 0L, long stop = -1L)
    {
        var values = await db.ListRangeAsync(new RedisKey(key), start, stop);
        return values.Select(v => JsonConvert.DeserializeObject<T>(v.ToString())!).ToArray();
    }
    
    public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(connectionString));
        services.AddSingleton<IDatabase>(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        return services;
    }
}
