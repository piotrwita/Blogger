using Application.Dto.Cosmos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.V2
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly ICosmosPostService _cosmosPostService;
        public PostsController(ICosmosPostService cosmosPostService)
        {
            _cosmosPostService =
                cosmosPostService ?? throw new ArgumentNullException(nameof(cosmosPostService));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _cosmosPostService.GetAllPostsAsync();
            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var post = await _cosmosPostService.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCosmosPostDto newPost)
        {
            var post = await _cosmosPostService.AddNewPostAsync(newPost);
            return Created($"api/posts/{post.Id}", post);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCosmosPostDto updatePost)
        {
            await _cosmosPostService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _cosmosPostService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
