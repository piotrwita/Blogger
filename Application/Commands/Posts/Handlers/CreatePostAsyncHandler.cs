using App.Metrics;
using Application.Commands.Posts;
using Application.Dto.Posts;
using Application.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Posts.Handlers
{
    public class CreatePostAsyncHandler : IRequestHandler<CreatePostAsyncCommand, PostDto>
    {
        private readonly IPostService _postService;
        private readonly ILogger<CreatePostAsyncHandler> _logger;
        private readonly IMetrics _metrics;

        public CreatePostAsyncHandler(IPostService postService, ILogger<CreatePostAsyncHandler> logger, IMetrics metrics)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));

            _metrics =
                metrics ?? throw new ArgumentNullException(nameof(metrics));
        }
        public async Task<PostDto> Handle(CreatePostAsyncCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.AddNewPostAsync(request.NewPost, request.UserId);
            _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedPostsCounter);
            _logger.LogInformation($"Created post: {request.NewPost.Title}");
            return post;
        }
    }
}