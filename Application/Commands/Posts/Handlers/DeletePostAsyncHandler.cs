using Application.Commands.Posts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Posts.Handlers
{
    public class DeletePostAsyncHandler : IRequestHandler<DeletePostAsyncCommand, Unit>
    {
        private readonly IPostService _postService;
        private readonly ILogger<DeletePostAsyncHandler> _logger;

        public DeletePostAsyncHandler(IPostService postService, ILogger<DeletePostAsyncHandler> logger)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeletePostAsyncCommand request, CancellationToken cancellationToken)
        {
            await _postService.DeletePostAsync(request.PostId);
            _logger.LogInformation($"Deleted post: {request.PostId}");
            return Unit.Value;
        }
    }
}