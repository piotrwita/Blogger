using Application.Dto.Posts;
using MediatR;

namespace Application.Commands.Posts
{
    public class UpdatePostAsyncCommand : IRequest
    {
        public UpdatePostDto UpdatePost { get; set; }

        public UpdatePostAsyncCommand(UpdatePostDto updatePost)
        {
            UpdatePost =
                updatePost ?? throw new ArgumentNullException(nameof(updatePost));
        }
    }
}
