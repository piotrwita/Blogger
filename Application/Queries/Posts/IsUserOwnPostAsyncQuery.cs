using MediatR;

namespace Application.Queries.Posts
{
    public class IsUserOwnPostAsyncQuery : IRequest<bool>
    {
        public int PostId { get; set; }
        public string UserId { get; set; }

        public IsUserOwnPostAsyncQuery(int postId, string userId)
        {
            PostId = postId;

            UserId =
                userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}
