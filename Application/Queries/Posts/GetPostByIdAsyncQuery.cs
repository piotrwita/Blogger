using Application.Dto.Posts;
using MediatR;

namespace Application.Queries.Posts
{
    public class GetPostByIdAsyncQuery : IRequest<PostDto>
    {
        public int Id { get; set; }

        public GetPostByIdAsyncQuery(int id)
        {
            Id = id;
        }
    }
}
