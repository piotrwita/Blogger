using Application.Commands.Posts;
using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Posts.Handlers
{
    public class GetAllPostsAsyncHandler : IRequestHandler<GetAllPostsQuery, IQueryable<PostDto>>
    {
        private readonly IPostService _postService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GetAllPostsAsyncHandler> _logger;
        public GetAllPostsAsyncHandler(IPostService postService, ILogger<GetAllPostsAsyncHandler> logger, IMemoryCache memoryCache)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _memoryCache =
                memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IQueryable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = _memoryCache.Get<IQueryable<PostDto>>("posts");

            if (posts == null)
            {
                _logger.LogInformation("Fetching from service"); 
                posts = _postService.GetAllPosts();
                //dodanie danych do cache, klucz pod ktorym przechowywane sa dane w pamieci, kolekcja danych, czas przez ktory dane sa cachowane
                _memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
            }
            else
            {
                _logger.LogInformation("Fetching from cache");
            } 

            _logger.LogInformation($"Get all posts");
            return Task.FromResult(posts);
        }
    }
}
