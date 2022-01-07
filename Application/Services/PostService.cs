using Application.Dto.Posts;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;

        public PostService(IPostRepository postRepository,
                           IMapper mapper,
                           ILogger<PostService> logger)
        {
            _postRepository =
                postRepository ?? throw new ArgumentNullException(nameof(postRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQueryable<PostDto> GetAllPosts()
        {
            var posts = _postRepository.GetAll();
            //bada zwracany model i generuje tylko kod sql potrzebny do zwracania odpowiednich pól
            return _mapper.ProjectTo<PostDto>(posts);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
            _logger.LogDebug("Fetching posts");
            _logger.LogInformation($"pageNumber: {pageNumber} | pageSize: {pageSize}");

            var posts = await _postRepository.GetAllAsync(pageNumber, pageSize, sortField, ascengind, filterBy);
            //zwracane są zawsze wszystkie pola określone przez model
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<int> GetAllPostsCountAsync(string filterBy)
        {
            return await _postRepository.GetAllCountAsync(filterBy);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost, string userId)
        {
            //if (string.IsNullOrEmpty(newPost.Title) && string.IsNullOrWhiteSpace(newPost.Title))
            //    throw new Exception("Post can not have an empty title");

            //if(newPost.Title.Length < 5 && newPost.Title.Length > 100)
            //    throw new Exception("The title must be between 5 and 100 characters long");

            var post = _mapper.Map<Post>(newPost);
            post.UserId = userId;
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

        public async Task<bool> UserOwnPostAsync(int postId, string userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null || post.UserId != userId)
                return false;

            return true;
        }
    }
}
