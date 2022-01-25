using Application.Dto.Posts;
using MediatR;

namespace Application.Queries.Posts
{
    public class GetAllPostsQuery : IRequest<IQueryable<PostDto>>
    {
        public GetAllPostsQuery()
        {
        }
    }
}
