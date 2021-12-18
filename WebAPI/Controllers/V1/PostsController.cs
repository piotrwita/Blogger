using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;
using WebAPI.Helpers;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //Zapamiętać, że async zmienić w Repository Services oraz Controllers
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] /*Wartość parametru zostanie pobrana z ciągu zapytania*/ PaginationFilter paginationFilter)
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber,paginationFilter.PageSize);

            var posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize);
            var totalRecords = await _postService.GetAllPostsCountAsync();
            var pagedResponse = PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords);

            return Ok(pagedResponse);
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            return Ok(new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Retrieves a specific posts by title")]
        [HttpGet("Serach/{title}")]
        public async Task<IActionResult> Get(string title, [FromQuery] PaginationFilter paginationFilter)
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);

            var posts = await _postService.SearchPostByTitleAsync(title, validPaginationFilter.PageNumber, validPaginationFilter.PageSize);            

            if (posts == null)
                return NotFound();

            var totalRecords = await _postService.GetAllPostsCountAsync();
            var pagedResponse = PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords);

            return Ok(pagedResponse);
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var post = await _postService.AddNewPostAsync(newPost);
            return Created($"api/posts/{post.Id}", new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostDto updatePost)
        {
            await _postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
