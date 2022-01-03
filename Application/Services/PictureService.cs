using Application.Dto.Picture;
using Application.ExtensionMethods;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PictureService(IPictureRepository pictureRepository,
                           IPostRepository postRepository,
                           IMapper mapper)
        {
            _pictureRepository =
                pictureRepository ?? throw new ArgumentNullException(nameof(pictureRepository));

            _postRepository =
                postRepository ?? throw new ArgumentNullException(nameof(postRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));
        } 

        public async Task<IEnumerable<PictureDto>> GetPituresByPostIdAsync(int postId)
        {
            var pictures = await _pictureRepository.GetByPostIdAsync(postId);
            return _mapper.Map<IEnumerable<PictureDto>>(pictures);
        }

        public async Task<PictureDto> GetPictureByIdAsync(int id)
        {
            var picture = await _pictureRepository.GetByIdAsync(id);
            return _mapper.Map<PictureDto>(picture);
        }

        public async Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            var existingPictures = await _pictureRepository.GetByPostIdAsync(postId);

            var posts = new List<Post>()
                            {
                                post
                            };
            var name = file.FileName;
            var image = file.GetBytes();
            var main = existingPictures.Count() == 0 ? true : false;

            var picture = new Picture()
            {
                Posts = posts,
                Name = name,
                Image = image,
                Main = main
            };

            var results = await _pictureRepository.AddAsync(picture);
            return _mapper.Map<PictureDto>(results);
        }

        public async Task SetMainPictureAsync(int postId, int id)
        {
            await _pictureRepository.SetMainPictureAsync(postId, id);  
        }

        public async Task DeletePictureAsync(int id)
        {
            var picture = await _pictureRepository.GetByIdAsync(id);
            await _pictureRepository.DeleteAsync(picture);
        }

    }
}
