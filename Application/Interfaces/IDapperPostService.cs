using Application.Dto.Post;

namespace Application.Interfaces
{
    public interface IDapperPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy);
        Task<int> GetAllPostsCountAsync(string filterBy);
        Task<PostDto> GetPostByIdAsync(int id);
        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
        Task UpdatePostAsync(UpdatePostDto updatePost);
        Task DeletePostAsync(int id);
    }
}
