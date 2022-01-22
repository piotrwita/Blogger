using Application.Dto.Posts;
using MediatR;
using WebAPI.Queries;

namespace WebAPI.Handlers
{
    public class GetAllPostsAsyncQuery :  : IRequest<IQueryable<PostDto>>
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
