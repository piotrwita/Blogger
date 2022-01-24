using Application.Dto.Posts;

namespace Application.Queries.Posts
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
