﻿using Application.Dto.Attachment;
using Application.Dto.Picture;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentDto>> GetAttachmentsByPostIdAsync(int postId);
        Task<DownloadAttachmentDto> DownloadAttachmentByIdAsync(int id);
        Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile file);
        Task DeleteAttachmentAsync(int id);
    }
}
