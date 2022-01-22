using MediatR;

namespace WebAPI.Handlers.Posts
{
    public class UpdatePostAsyncHandler : IRequestHandler<GetAllPostsQuery, IQueryable<PostDto>>
    {
    }
}
