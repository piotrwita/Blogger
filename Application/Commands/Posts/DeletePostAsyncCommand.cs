using MediatR;

namespace Application.Commands.Posts
{
    public class DeletePostAsyncCommand : IRequest
    {
        public int PostId { get; set; }

        public DeletePostAsyncCommand(int postId)
        {
            PostId = postId;
        }
    }
}
