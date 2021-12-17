using Domain.Interfaces;

namespace Application.Services
{
    public class CosmosPostService : ICosmosPostService
    {
        private readonly ICosmosPostRepository _cosmosPostRepository;
        private readonly IMapper _mapper;
        public CosmosPostService(ICosmosPostRepository cosmosPostRepository,
                           IMapper mapper)
        {
            _cosmosPostRepository =
                cosmosPostRepository ?? throw new ArgumentNullException(nameof(cosmosPostRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CosmosPostDto>> GetAllPostsAsync()
        {
            var posts = await _cosmosPostRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CosmosPostDto>>(posts);
        }

        public async Task<CosmosPostDto> GetPostByIdAsync(string id)
        {
            var post = await _cosmosPostRepository.GetByIdAsync(id);
            return _mapper.Map<CosmosPostDto>(post);
        }

        public async Task<IEnumerable<CosmosPostDto>> SearchPostByTitleAsync(string title)
        {
            var lowerTitle = title.ToLowerInvariant();
            var posts = await _cosmosPostRepository.GetAllAsync();
            //czy to jest ok?
            var results = posts.Where(post => post.Title.ToLowerInvariant().Contains(lowerTitle));
            return _mapper.Map<IEnumerable<CosmosPostDto>>(results);
        }

        public async Task<CosmosPostDto> AddNewPostAsync(CreateCosmosPostDto newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title))
                throw new Exception("Post can not have an empty title");

            var post = _mapper.Map<CosmosPost>(newPost);
            var results = await _cosmosPostRepository.AddAsync(post);
            return _mapper.Map<CosmosPostDto>(results);
        }

        public async Task UpdatePostAsync(UpdateCosmosPostDto updatePost)
        {
            var existingPost = await _cosmosPostRepository.GetByIdAsync(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            await _cosmosPostRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(string id)
        {
            var post = await _cosmosPostRepository.GetByIdAsync(id);
            await _cosmosPostRepository.DeleteAsync(post);
        }
    }
}
