using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly BloggerContext _context;

        public PictureRepository(BloggerContext context)
        {
            _context =
                context ?? throw new ArgumentException(nameof(context));
        }  

        public async Task<IEnumerable<Picture>> GetByPostIdAsync(int postId)
        {
            //ładowanie obiektów podrzednych za pomocą metody rozszerzaącej include
            //dzieki temu możemy się w warunku where mozemy się odwołać do podrzędnej kolekcji obiektów
            return await _context.Pictures
                .Include(p => p.Posts)
                .Where(p => p.Posts
                    .Select(p => p.Id)
                    .Contains(postId))
                .ToListAsync();
        }

        public async Task<Picture> GetByIdAsync(int id)
        {
            return await _context.Pictures.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Picture> AddAsync(Picture picture)
        {
            var createdPicture = await _context.Pictures.AddAsync(picture);
            await _context.SaveChangesAsync();

            return createdPicture.Entity;
        }

        public async Task SetMainPictureAsync(int postId, int id)
        {
            var currentMainPicture = await _context.Pictures
                .Include(p => p.Posts)
                .Where(p => p.Posts
                    .Select(p => p.Id)
                    .Contains(postId))
                .SingleOrDefaultAsync(p => p.Main);

            currentMainPicture.Main = false;

            var newMainPicture = await GetByIdAsync(id);

            newMainPicture.Main = true;

            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Picture picture)
        {
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
