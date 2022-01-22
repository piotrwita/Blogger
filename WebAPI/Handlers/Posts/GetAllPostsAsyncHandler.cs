using Application.Dto.Posts;
using MediatR;
using WebAPI.Queries;

namespace WebAPI.Handlers.Posts
{
    public class GetAllPostsAsyncHandler : IRequestHandler<GetAllPostsQuery, IQueryable<PostDto>>
    {
        public GetAllPostsAsyncHandler()
        {
        }

        public async Task<IQueryable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
