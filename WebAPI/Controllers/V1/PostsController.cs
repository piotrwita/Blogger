﻿using Application.Dto;
using Application.Dto.Post;
using Application.Interfaces;
using Infrastructure.Identity;
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
    //określona rola z dostepem do żądanego zasobu z konrolera identity 
    //nadrzedny
    [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin + "," + UserRoles.SuperUser)]
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
        public async Task<IActionResult> GetAsync([FromQuery] /*Wartość parametru zostanie pobrana z ciągu zapytania*/ PaginationFilter paginationFilter,
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
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperUser)]
        //[EnableQuery]
        [HttpGet("[action]")]
        public IQueryable<PostDto> GetAll()
        {
            return _postService.GetAllPosts();
        }

        [SwaggerOperation(Summary = "Retrieves a specyfic post by unique id")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            var respone = new Response<PostDto>(post);

            return Ok(respone);
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.SuperUser)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePostDto newPost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _postService.AddNewPostAsync(newPost, userId);

            var response = new Response<PostDto>(post);

            return Created($"api/posts/{post.Id}", response);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.SuperUser)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdatePostDto updatePost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.UserOwnPostAsync(updatePost.Id, userId);
            var isSuperUser = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.SuperUser);

            if (!userOwnPost && !isSuperUser)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }
                
            await _postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specyfic post")]
        [Authorize(Roles = UserRoles.AdminOrUser + "," + UserRoles.SuperUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOwnPost = await _postService.UserOwnPostAsync(id, userId);
            var isAdmin = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.Admin);
            var isSuperUser = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.SuperUser);

            if (!userOwnPost && !isAdmin && !isSuperUser)
            {
                var succeeded = false;
                var message = "You do not own this post";

                var response = new Response(succeeded, message);

                return BadRequest(response);
            }

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
