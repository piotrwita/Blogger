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

        public IQueryable<PostDto> GetAllPosts()
        {
            var posts = _postRepository.GetAll();
            //bada zwracany model i generuje tylko kod sql potrzebny do zwracania odpowiednich pól
            return _mapper.ProjectTo<PostDto>(posts);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
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
