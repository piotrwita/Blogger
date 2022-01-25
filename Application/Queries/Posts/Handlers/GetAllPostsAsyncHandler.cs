using Application.Commands.Posts;
using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Posts.Handlers
{
    public class GetAllPostsAsyncHandler : IRequestHandler<GetAllPostsQuery, IQueryable<PostDto>>
    {
        private readonly IPostService _postService;
        private readonly ILogger<GetAllPostsQuery> _logger;
        public GetAllPostsAsyncHandler(IPostService postService, ILogger<GetAllPostsQuery> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IQueryable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = _postService.GetAllPosts();
            _logger.LogInformation($"Get all posts");
            return Task.FromResult(posts);
        }
    }
}
