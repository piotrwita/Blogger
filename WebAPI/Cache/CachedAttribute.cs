using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace WebAPI.Cache
{
    //ograniczenie dla atrybutu, by mogl byc wykorzystany tylko na poziomie klasy,metody
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        //okresla przez jak iczas dane beda przechwoywane w pamieci podrecznej redis
        private readonly int _timeToLiveSecounds;

        public CachedAttribute(int timeToLiveSecounds)
        {
            _timeToLiveSecounds = timeToLiveSecounds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //to co wykona sie przed przejsciem do nastepnego middleware
            //w tym miejscu sprawdzamy czy dane sa cacheowane i jesli tak to je zwrocic
            //jesli nie to przechodzimy do kolejnego middleware a co za tym idzie do wykonania akcji controllera
            //na koniec operacje ktore musza sie wykonac po powrocie z kolejnych middleware (cache danych)

            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (cacheSettings == null)
            {
                await next();
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            //klucz po to zeby wiedziec z ktorego cache mamy pobrac dane (atrybut moze byc przypisany do wielu metod)
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            //jezeli zadne dane nie byly cachowane musimy je zachowac
            var executedContext = await next();
            if(executedContext.Result is ObjectResult okObjectResult)
            {
                await cacheService.CacheRespnoseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSecounds));
            }
        }

        /// <summary>
        /// tworzony zostaje unikalny klucz na podstawie ścieżki oraz zapytania pochodzacych z żądania
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
