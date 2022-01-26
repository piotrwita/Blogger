using Application.Commands.Posts;
using Application.Dto.Posts;
using Application.Queries.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Posts
{
    public class IsUserOwnPostAsyncHandler : IRequestHandler<IsUserOwnPostAsyncQuery, bool>
    {
        private readonly IPostService _postService;
        private readonly ILogger<IsUserOwnPostAsyncHandler> _logger;

        public IsUserOwnPostAsyncHandler(IPostService postService, ILogger<IsUserOwnPostAsyncHandler> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(IsUserOwnPostAsyncQuery request, CancellationToken cancellationToken)
        {
            var isUserOwnPost = await _postService.IsUserOwnPostAsync(request.PostId, request.UserId);
            _logger.LogInformation($"Check if the user is the owner of the post (PostId: {request.PostId}, UserId: {request.UserId}");
            return isUserOwnPost;
        }
    }
}