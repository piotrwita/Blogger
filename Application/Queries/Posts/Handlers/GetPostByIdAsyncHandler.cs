using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Posts.Handlers
{
    public class GetPostByIdAsyncHandler : IRequestHandler<GetPostByIdAsyncQuery, PostDto>
    {
        private readonly IPostService _postService;
        private readonly ILogger<GetPostByIdAsyncHandler> _logger;
        public GetPostByIdAsyncHandler(IPostService postService, ILogger<GetPostByIdAsyncHandler> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PostDto> Handle(GetPostByIdAsyncQuery request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.Id);
            _logger.LogInformation($"Get post by id: {request.Id}");
            return post;
        }
    }
}

