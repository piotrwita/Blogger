using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Posts.Handlers
{
    public class GetAllPostsCountAsyncHandler : IRequestHandler<GetAllPostsCountAsyncQuery, int>
    {
        private readonly IPostService _postService;
        private readonly ILogger<GetAllPostsCountAsyncQuery> _logger;
        public GetAllPostsCountAsyncHandler(IPostService postService, ILogger<GetAllPostsCountAsyncQuery> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(GetAllPostsCountAsyncQuery request, CancellationToken cancellationToken)
        {
            var count = await _postService.GetAllPostsCountAsync(request.FilterBy);
            _logger.LogInformation($"Get all posts count filered by: {request.FilterBy}");
            return count;
        }
    }
}

