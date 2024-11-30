using System.Text;
using Newtonsoft.Json;

namespace GameService.Abstraction;

public static class Extensions
{
    public static async Task<string> GetResultAsync(this HttpClient client, string url)
    {
        return await client.GetStringAsync(url);
    }
    
    public static async Task<ApiResult?> GetApiResultAsync(this HttpClient client, string url)
    {
        var result = await client.GetResultAsync(url);
        return JsonConvert.DeserializeObject<ApiResult>(result);
    }
    
    public static async Task<ApiResult<T>?> GetApiResultAsync<T>(this HttpClient client, string url)
    {
        var result = await client.GetResultAsync(url);
        return JsonConvert.DeserializeObject<ApiResult<T>>(result);
    }
    
    public static async Task<string> PostResultAsync(this HttpClient client, string url, HttpContent? content, CancellationToken token)
    {
        var response = await client.PostAsync(url, content, token);
        return await response.Content.ReadAsStringAsync(token);
    }
    
    public static async Task<ApiResult?> PostApiResultAsync(this HttpClient client, string url, HttpContent? content, CancellationToken token)
    {
        var result = await client.PostResultAsync(url, content, token);
        return JsonConvert.DeserializeObject<ApiResult>(result);
    }
    
    public static async Task<ApiResult<T>?> PostApiResultAsync<T>(this HttpClient client, string url, HttpContent? content, CancellationToken token)
    {
        var result = await client.PostResultAsync(url, content, token);
        return JsonConvert.DeserializeObject<ApiResult<T>>(result);
    }
}