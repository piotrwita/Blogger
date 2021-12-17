namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(int id);
        Task<IEnumerable<PostDto>> SearchPostByTitleAsync(string title);
        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
        Task UpdatePostAsync(UpdatePostDto updatePost);
        Task DeletePostAsync(int id);
    }
}
