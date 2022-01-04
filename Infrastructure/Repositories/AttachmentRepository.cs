using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly BloggerContext _context;

        public AttachmentRepository(BloggerContext context)
        {
            _context =
                context ?? throw new ArgumentException(nameof(context));
        }  

        public async Task<IEnumerable<Attachment>> GetByPostIdAsync(int postId)
        {
            //ładowanie obiektów podrzednych za pomocą metody rozszerzaącej include
            //dzieki temu możemy się w warunku where mozemy się odwołać do podrzędnej kolekcji obiektów
            return await _context.Attachments
                .Include(p => p.Posts)
                .Where(p => p.Posts
                    .Select(p => p.Id)
                    .Contains(postId))
                .ToListAsync();
        }

        public async Task<Attachment> GetByIdAsync(int id)
        {
            return await _context.Attachments.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Attachment> AddAsync(Attachment attachment)
        {
            var createdPicture = await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();

            return createdPicture.Entity;
        }

        public async Task DeleteAsync(Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
