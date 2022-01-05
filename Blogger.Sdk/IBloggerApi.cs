using Blogger.Contracts.Requests.Blogger;
using Blogger.Contracts.Requests.Responses;
using Blogger.Contracts.Responses;
using Refit;

namespace Blogger.Sdk
{
    //aby prawidłowo się uwieżytelnić (typ autoryzacji)
    [Headers("Authorization: Bearer")]
    public interface IBloggerApi
    {
        [Get("/api/posts/{id}")]
        Task<ApiResponse<Response<PostDto>>> GetPostAsync([Body] int id);

        [Post("/api/posts")]
        Task<ApiResponse<Response<PostDto>>> CreatePostAsync([Body] CreatePostDto newPost);

        [Put("/api/posts")]
        Task UpdatePostAsync([Body] UpdatePostDto updatePost);

        [Delete("/api/posts/{id}")]
        Task DeletePostAsync([Body] int id);
    }
}
