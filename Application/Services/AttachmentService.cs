using Application.Dto.Attachments;
using Application.Dto.Pictures;
using Application.ExtensionMethods;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public AttachmentService(IAttachmentRepository attachmentRepository,
                           IPostRepository postRepository,
                           IMapper mapper)
        {
            _attachmentRepository =
                attachmentRepository ?? throw new ArgumentNullException(nameof(attachmentRepository));

            _postRepository =
                postRepository ?? throw new ArgumentNullException(nameof(postRepository));

            _mapper =
                mapper ?? throw new ArgumentNullException(nameof(mapper));
        } 

        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByPostIdAsync(int postId)
        {
            var attachments = await _attachmentRepository.GetByPostIdAsync(postId);
            return _mapper.Map<IEnumerable<AttachmentDto>>(attachments);
        }

        public async Task<DownloadAttachmentDto> DownloadAttachmentByIdAsync(int id)
        {
            var existingAttachment = await _attachmentRepository.GetByIdAsync(id);

            var name = existingAttachment.Name;
            var content = File.ReadAllBytes(existingAttachment.Path);

            var attachment = new DownloadAttachmentDto
                            {
                                Name = name,
                                Content = content
                            };

            return attachment;
        }

        public async Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile file)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            var posts = new List<Post>()
                            {
                                post
                            };
            var name = file.FileName;
            var path = file.SaveFile();

            var attachment = new Attachment()
            {
                Posts = posts,
                Name = name,
                Path = path
            };

            var results = await _attachmentRepository.AddAsync(attachment);
            return _mapper.Map<AttachmentDto>(results);
        }

        public async Task DeleteAttachmentAsync(int id)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id);
            await _attachmentRepository.DeleteAsync(attachment);
        }

    }
}
