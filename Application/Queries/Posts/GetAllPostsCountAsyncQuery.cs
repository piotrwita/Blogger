using Application.Dto.Posts;
using MediatR;

namespace Application.Queries.Posts
{
    public class GetAllPostsCountAsyncQuery : IRequest<int>
    {
        public string FilterBy { get; set; }

        public GetAllPostsCountAsyncQuery(string filterBy)
        {
            FilterBy =
                filterBy ?? throw new ArgumentNullException(nameof(filterBy));
        }
    }
}
