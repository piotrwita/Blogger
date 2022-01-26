using Application.Commands.Posts;
using Application.Dto.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Posts.Handlers
{
    public class CreatePostAsyncHandler : IRequestHandler<CreatePostAsyncCommand, PostDto>
    {
        private readonly IPostService _postService;
        private readonly ILogger<CreatePostAsyncHandler> _logger;

        public CreatePostAsyncHandler(IPostService postService, ILogger<CreatePostAsyncHandler> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PostDto> Handle(CreatePostAsyncCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.AddNewPostAsync(request.NewPost, request.UserId);
            _logger.LogInformation($"Created post: {request.NewPost.Title}");
            return post;
        }
    }
}