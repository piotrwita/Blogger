﻿using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
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
    [Authorize]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService =
                postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [SwaggerOperation(Summary = "Retrieves sort fields")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetSortFields()
        {
            return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] /*Wartość parametru zostanie pobrana z ciągu zapytania*/ PaginationFilter paginationFilter,
                                                    [FromQuery] SortingFilter sortingFilter,
                                                    [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber,paginationFilter.PageSize);
            var validSortFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascengind);

            var posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber,
                                                            validPaginationFilter.PageSize,
                                                            validSortFilter.SortField, 
                                                            validSortFilter.Ascengind,
                                                            filterBy);

            var totalRecords = await _postService.GetAllPostsCountAsync(filterBy);
            var pagedResponse = PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords);

            return Ok(pagedResponse);
        }

        [SwaggerOperation(Summary = "Retrieves paged posts")]
        //[EnableQuery]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IQueryable<PostDto> GetAll()
        {
            return _postService.GetAllPosts();
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            return Ok(new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var post = await _postService.AddNewPostAsync(newPost, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created($"api/posts/{post.Id}", new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostDto updatePost)
        {
            var userOwnPost = await _postService.UserOwnPostAsync(updatePost.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if (!userOwnPost)
            {
                var response = new Response<bool>
                {
                    Succeeded = false,
                    Message = "You do not own this post"
                };

                return BadRequest(response);
            }
                
            await _postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userOwnPost = await _postService.UserOwnPostAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!userOwnPost)
            {
                var response = new Response<bool>
                {
                    Succeeded = false,
                    Message = "You do not own this post"
                };

                return BadRequest(response);
            }

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
