using Application.Dto.Attachments;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Claims;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.User)]
    [ApiController]
    public class AttachmentsController : Controller
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IPostService _postService;
        public AttachmentsController(IAttachmentService attachmentService,
                           IPostService postService)
        {
            _attachmentService =
                attachmentService ?? throw new ArgumentNullException(nameof(attachmentService));

            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [SwaggerOperation(Summary = "Retrieves a attachments by unique post id")]
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetPostIdAsync(int postId)
        {
            var attachments = await _attachmentService.GetAttachmentsByPostIdAsync(postId);

            var response = new Response<IEnumerable<AttachmentDto>>(attachments);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Download a specyfic attachment by unique id")]
        [HttpGet("{postId}/{id}")]
        public async Task<IActionResult> DownloadAsync(int id, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.UserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var attachment = await _attachmentService.DownloadAttachmentByIdAsync(id);
            if (attachment == null)
                return NotFound();

            var content = attachment.Content;
            //typ pliku - Octet to typ ogolny, nie okreslamy jakieo typu pliki wybieramy
            var type = MediaTypeNames.Application.Octet;
            var name = attachment.Name;

            return File(content, type, name);
        }

        [SwaggerOperation(Summary = "Add a new attachment to post")]
        [HttpPost("{postId}")]
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
            var userOwnPost = await _postService.UserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var attachment = await _attachmentService.AddAttachmentToPostAsync(postId, file);
            var respone = new Response<AttachmentDto>(attachment);

            return Created($"api/attachments/{attachment.Id}", respone);
        }

        [SwaggerOperation(Summary = "Delete a specyfic attachment")]
        [HttpDelete("{postId}/{id}")]
        public async Task<IActionResult> Delete(int id, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.UserOwnPostAsync(postId, userId);

            if (!userOwnPost)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            await _attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }
    }
}
