using Domain.Interfaces;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository,
                           IMapper mapper)
        {
            _postRepository =
                postRepository ?? throw new ArgumentNullException(nameof(postRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize)
        {
            var posts = await _postRepository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<int> GetAllPostsCountAsync()
        {
            return await _postRepository.GetAllCountAsync();
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<IEnumerable<PostDto>> SearchPostByTitleAsync(string title, int pageNumber, int pageSize)
        {
            var lowerTitle = title.ToLowerInvariant();
            var posts = await _postRepository.GetAllAsync(pageNumber, pageSize);
            //czy to jest ok?
            var results = posts.Where(post => post.Title.ToLowerInvariant().Contains(lowerTitle));
            return _mapper.Map<IEnumerable<PostDto>>(results);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title))
                throw new Exception("Post can not have an empty title");

            var post = _mapper.Map<Post>(newPost);
            var results = await _postRepository.AddAsync(post);
            return _mapper.Map<PostDto>(results);
        }

        public async Task UpdatePostAsync(UpdatePostDto updatePost)
        {
            var existingPost = await _postRepository.GetByIdAsync(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            await _postRepository.DeleteAsync(post);
        }
    }
}
