using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = 
                distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task CacheRespnoseAsync(string cacheKey, object response, TimeSpan timeLive)
        {
            if (response == null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);

            var options = new DistributedCacheEntryOptions
            {
                //okresla czas przez jaki dane maja byc cachowane
                AbsoluteExpirationRelativeToNow = timeLive
            };

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, options);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            var results = String.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
            return results;
        }
    }
}
