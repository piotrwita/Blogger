using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Posts.Handlers
{
    public class GetPagedPostsAsyncHandler : IRequestHandler<GetPagedPostsAsyncQuery, IEnumerable<PostDto>>
    {
        private readonly IPostService _postService;
        private readonly ILogger<GetPagedPostsAsyncQuery> _logger;
        public GetPagedPostsAsyncHandler(IPostService postService, ILogger<GetPagedPostsAsyncQuery> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPagedPostsAsyncQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postService.GetAllPostsAsync(request.PageNumber, request.PageSize, request.SortField, request.Ascengind, request.FilterBy);
            _logger.LogInformation($"Get paged posts [pn:{request.PageNumber} | ps:{request.PageSize} | sf:{request.SortField} | a:{request.Ascengind} | fb:{request.FilterBy}]");
            return posts;
        }
    }
}

