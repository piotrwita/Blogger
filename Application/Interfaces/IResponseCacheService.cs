namespace Application.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheRespnoseAsync(string cacheKey, object response, TimeSpan timeLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
