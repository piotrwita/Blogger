namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize);
        Task<int> GetAllPostsCountAsync();
        Task<PostDto> GetPostByIdAsync(int id);
        Task<IEnumerable<PostDto>> SearchPostByTitleAsync(string title, int pageNumber, int pageSize);
        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
        Task UpdatePostAsync(UpdatePostDto updatePost);
        Task DeletePostAsync(int id);
    }
}
