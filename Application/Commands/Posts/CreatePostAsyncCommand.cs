using Application.Dto.Posts;
using MediatR;

namespace Application.Commands.Posts
{
    public class CreatePostAsyncCommand : IRequest<PostDto>
    {
        public CreatePostDto NewPost { get; set; }
        public string UserId { get; set; }

        public CreatePostAsyncCommand(CreatePostDto newPost, string userId)
        {
            NewPost =
                newPost ?? throw new ArgumentNullException(nameof(newPost));

            UserId =
                userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}
