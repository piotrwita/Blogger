using Application.Dto.Post;
using Domain.Interfaces;

namespace Application.Services
{
    public class DapperPostService : IDapperPostService
    {
        private readonly IDapperPostRepository _dapperPostRepository;
        private readonly IMapper _mapper;
        public DapperPostService(IDapperPostRepository dapperPostRepository,
                           IMapper mapper)
        {
            _dapperPostRepository =
                dapperPostRepository ?? throw new ArgumentNullException(nameof(dapperPostRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
            var posts = await _dapperPostRepository.GetAllAsync(pageNumber, pageSize, sortField, ascengind, filterBy);
            //zwracane są zawsze wszystkie pola określone przez model
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<int> GetAllPostsCountAsync(string filterBy)
        {
            return await _dapperPostRepository.GetAllCountAsync(filterBy);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await _dapperPostRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title))
                throw new Exception("Post can not have an empty title");

            var post = _mapper.Map<Post>(newPost);
            var results = await _dapperPostRepository.AddAsync(post);
            return _mapper.Map<PostDto>(results);
        }

        public async Task UpdatePostAsync(UpdatePostDto updatePost)
        {
            var existingPost = await _dapperPostRepository.GetByIdAsync(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            await _dapperPostRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _dapperPostRepository.GetByIdAsync(id);
            await _dapperPostRepository.DeleteAsync(post);
        }
    }
}
