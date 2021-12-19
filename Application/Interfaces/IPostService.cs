﻿namespace Application.Interfaces
{
    public interface IPostService
    {
        IQueryable<PostDto> GetAllPosts();
        Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy);
        Task<int> GetAllPostsCountAsync(string filterBy);
        Task<PostDto> GetPostByIdAsync(int id);
        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
        Task UpdatePostAsync(UpdatePostDto updatePost);
        Task DeletePostAsync(int id);
    }
}
