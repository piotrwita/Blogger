using Application.Dto.Pictures;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.User)]
    [ApiController]
    public class PicturesController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly IPostService _postService;
        public PicturesController(IPictureService pictureService,
                           IPostService postService)
        {
            _pictureService =
                pictureService ?? throw new ArgumentNullException(nameof(pictureService));

            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [SwaggerOperation(Summary = "Retrieves a pictures by unique post id")]
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetPostIdAsync(int postId)
        {
            var pictures = await _pictureService.GetPituresByPostIdAsync(postId);

            var response = new Response<IEnumerable<PictureDto>>(pictures);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic picture by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var picture = await _pictureService.GetPictureByIdAsync(id);

            if (picture == null)
            {
                return NotFound();
            }

            var response = new Response<PictureDto>(picture);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Add a new picture to post")]
        [HttpPost("postId")]
        public async Task<IActionResult> AddToPostAsync(int postId, IFormFile file)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {
                var succeeded = false;
                var message = $"Post with id {postId} does not exist";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.IsUserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var picture = await _pictureService.AddPictureToPostAsync(postId, file);
            var respone = new Response<PictureDto>(picture);

            return Created($"api/pictures/{picture.Id}", respone);
        }

        [SwaggerOperation(Summary = "Sets the main picture of the post")]
        [HttpPut("[action]/{postId}/{id}")]
        public async Task<IActionResult> SetMainPictureAsync(int postId, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.IsUserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            await _pictureService.SetMainPictureAsync(postId, id);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic picture")]
        [HttpDelete("{postId}/{id}")]
        public async Task<IActionResult> DeleteAsync(int id, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.IsUserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            await _pictureService.DeletePictureAsync(id);
            return NoContent();
        }
    }
}
