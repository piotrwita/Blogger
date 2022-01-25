using Application.Dto.Posts;
using MediatR;

namespace Application.Queries.Posts
{
    public class GetPagedPostsAsyncQuery : IRequest<IEnumerable<PostDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public bool Ascengind { get; set; }
        public string FilterBy { get; set; }

        public GetPagedPostsAsyncQuery(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
            PageNumber = pageNumber;

            PageSize = pageSize;

            SortField = 
                sortField ?? throw new ArgumentNullException(nameof(sortField));

            Ascengind = ascengind;
            
            FilterBy =
                filterBy ?? throw new ArgumentNullException(nameof(filterBy));
        }
    }
}
