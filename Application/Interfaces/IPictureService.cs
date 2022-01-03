using Application.Dto.Picture;
using Application.Dto.Post;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureDto>> GetPituresByPostIdAsync(int postId);
        Task<PictureDto> GetPictureByIdAsync(int id);
        Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file);
        Task SetMainPictureAsync(int postId, int id);
        Task DeletePictureAsync(int id);
    }
}
