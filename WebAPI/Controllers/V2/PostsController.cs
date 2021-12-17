using Application.Dto;
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
        //private readonly IPostService _postServices;
        //public PostsController(IPostService postServices)
        //{
        //    _postServices =
        //        postServices ?? throw new ArgumentNullException(nameof(postServices));
        //}

        //[SwaggerOperation(Summary = "Retrieves all posts")]
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var posts = _postServices.GetAllPosts();
        //    return Ok(
        //        new
        //        {
        //            Posts = posts,
        //            Count = posts.Count()
        //        });
        //}

        //[SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    var post = _postServices.GetPostById(id);

        //    if (post == null)
        //        return NotFound();

        //    return Ok(post);
        //}

        //[SwaggerOperation(Summary = "Create a new post")]
        //[HttpPost]
        //public IActionResult Create(CreatePostDto newPost)
        //{
        //    var post = _postServices.AddNewPostAs(newPost);
        //    return Created($"api/posts/{post.Id}",post);
        //}

        //[SwaggerOperation(Summary = "Update a existing post")]
        //[HttpPut]
        //public IActionResult Update(UpdatePostDto updatePost)
        //{
        //    _postServices.UpdatePost(updatePost);
        //    return NoContent();
        //}

        //[SwaggerOperation(Summary = "Delete a specyfic post")]
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    _postServices.DeletePost(id);
        //    return NoContent();
        //}
    }
}
