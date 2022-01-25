using Application.Commands.Posts;
using Application.Dto.Posts;
using Application.Handlers.Posts;
using Application.Interfaces;
using Application.Queries.Posts;
using Application.Queries.Posts.Handlers;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Attributes;
using WebAPI.Cache;
using WebAPI.Filters;
using WebAPI.Helpers;

using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //Zapamiętać, że async zmienić w Repository Services oraz Controllers
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //umozliwa ograniczenie dostepu do zasobów na podstawie ról
    //określona rola z dostepem do żądanego zasobu z konrolera identity 
    //nadrzedny
    [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin + "," + UserRoles.SuperUser)]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<PostsController> _logger;
        private readonly IMediator _mediator;
        public PostsController(IPostService postService,
                               IMemoryCache memoryCache,
                               ILogger<PostsController> logger,
                               IMediator mediator)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));

            _memoryCache =
                memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger));

            _mediator =
                mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [SwaggerOperation(Summary = "Retrieves sort fields")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetSortFields()
        {
            return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
        }

        [SwaggerOperation(Summary = "Retrieves paged posts")]
        [AllowAnonymous]
        //[Cached(600)]
        [HttpGet]
        public async Task<IActionResult> GetPagedAsync([FromQuery] /*Wartość parametru zostanie pobrana z ciągu zapytania*/ PaginationFilter paginationFilter,
                                                    [FromQuery] SortingFilter sortingFilter,
                                                    [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascengind);

            var postsQuery = new GetPagedPostsAsyncQuery(validPaginationFilter.PageNumber,
                                                    validPaginationFilter.PageSize,
                                                    validSortFilter.SortField,
                                                    validSortFilter.Ascengind,
                                                    filterBy);
            var posts = await _mediator.Send(postsQuery);

            var totalRecordsQuery = new GetAllPostsCountAsyncQuery(filterBy);
            var totalRecords = await _mediator.Send(totalRecordsQuery); 

            var pagedResponse = PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords);

            return Ok(pagedResponse);
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperUser)]
        //[EnableQuery]
        [HttpGet("[action]")]
        public IQueryable<PostDto> GetAll()
        {
            var posts = _memoryCache.Get<IQueryable<PostDto>>("posts");

            if (posts == null)
            {
                _logger.LogInformation("Fetching from service");
                var query = new GetAllPostsQuery(); 
                posts = _mediator.Send(query).Result;
                //dodanie danych do cache, klucz pod ktorym przechowywane sa dane w pamieci, kolekcja danych, czas przez ktory dane sa cachowane
                _memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
            }
            else
            {
                _logger.LogInformation("Fetching from cache");
            }

            return posts;
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var query = new GetPostByIdAsyncQuery(id);
            var post = await _mediator.Send(query); 

            if (post == null)
                return NotFound();

            var respone = new Response<PostDto>(post);

            return Ok(respone);
        }

        [ValidateFilter]
        [SwaggerOperation(Summary = "Create a new post")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.SuperUser)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePostDto newPost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CreatePostAsyncCommand(newPost, userId);
            var post = await _mediator.Send(command);

            var response = new Response<PostDto>(post);

            return Created($"api/posts/{post.Id}", response);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.SuperUser)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdatePostDto updatePost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new IsUserOwnPostAsyncQuery(updatePost.Id, userId);
            var isUserOwnPost = await _mediator.Send(query);

            var isSuperUser = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.SuperUser);

            if (!isUserOwnPost && !isSuperUser)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var command = new UpdatePostAsyncCommand(updatePost);
            await _mediator.Send(command);

            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [Authorize(Roles = UserRoles.AdminOrUser + "," + UserRoles.SuperUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new IsUserOwnPostAsyncQuery(id, userId);
            var isUserOwnPost = await _mediator.Send(query);

            var isAdmin = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.Admin);
            var isSuperUser = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.SuperUser);

            if (!isUserOwnPost && !isAdmin && !isSuperUser)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            var command = new DeletePostAsyncCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
