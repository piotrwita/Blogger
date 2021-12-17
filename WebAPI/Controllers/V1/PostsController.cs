using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.V1
{

    //Zapamiętać, że async zmienić w Repository Services oraz Controllers
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postServices;
        public PostsController(IPostService postServices)
        {
            _postServices =
                postServices ?? throw new ArgumentNullException(nameof(postServices));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var posts = await _postServices.GetAllPostsAsync();
            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postServices.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [SwaggerOperation(Summary = "Retrieves a specific posts by title")]
        [HttpGet("Serach/{title}")]
        public async Task<IActionResult> Get(string title)
        {
            var posts = await _postServices.SearchPostByTitleAsync(title);

            if (posts == null)
                return NotFound();

            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var post = await _postServices.AddNewPostAsync(newPost);
            return Created($"api/posts/{post.Id}",post);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostDto updatePost)
        {
            await _postServices.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _postServices.DeletePostAsync(id);
            return NoContent();
        }
    }
}
