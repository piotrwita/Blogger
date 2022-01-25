using Application.Commands.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Posts.Handlers
{
    public class UpdatePostAsyncHandler : IRequestHandler<UpdatePostAsyncCommand, Unit>
    {
        private readonly IPostService _postService;
        private readonly ILogger<UpdatePostAsyncCommand> _logger;

        public UpdatePostAsyncHandler(IPostService postService, ILogger<UpdatePostAsyncCommand> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdatePostAsyncCommand request, CancellationToken cancellationToken)
        {
            await _postService.UpdatePostAsync(request.UpdatePost);
            _logger.LogInformation($"Updated post: {request.UpdatePost.Id}");
            return Unit.Value;
        }
    }
}