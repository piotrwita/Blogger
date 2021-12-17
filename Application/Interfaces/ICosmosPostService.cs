namespace Application.Interfaces
{
    public interface ICosmosPostService
    {
        Task<IEnumerable<CosmosPostDto>> GetAllPostsAsync();
        Task<CosmosPostDto> GetPostByIdAsync(string id);
        Task<IEnumerable<CosmosPostDto>> SearchPostByTitleAsync(string title);
        Task<CosmosPostDto> AddNewPostAsync(CreateCosmosPostDto newPost);
        Task UpdatePostAsync(UpdateCosmosPostDto updatePost);
        Task DeletePostAsync(string id);
    }
}
