using Blogger.Contracts.Requests.Identity;
using Blogger.Contracts.Responses;
using Refit;

namespace Blogger.Sdk
{
    public interface IIdentityApi
    {
        [Post("/api/identity/register")]
        Task<ApiResponse<Response>> RegisterAsync([Body] RegisterModel register);

        [Post("/api/identity/registerAdmin")]
        Task<ApiResponse<Response>> RegisterAdminAsync([Body] RegisterModel register);

        [Post("/api/identity/registerSuperUser")]
        Task<ApiResponse<Response>> RegisterSuperUserAsync([Body] RegisterModel register);

        [Post("/api/identity/login")]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] LoginModel login);
    }
}
