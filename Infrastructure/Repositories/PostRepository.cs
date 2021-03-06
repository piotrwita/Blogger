using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        //private static readonly ISet<Post> _posts = new HashSet<Post>()
        //{
        //    new Post(1,"Tytuł 1","Treść 1"),
        //    new Post(2,"Tytuł 2","Treść 2"),
        //    new Post(3,"Tytuł 3","Treść 3")
        //};

        
        private readonly BloggerContext _context;

        #region Contructor

        public PostRepository(BloggerContext context)
        {
            _context = 
                context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region IPostRepository
        public IQueryable<Post> GetAll()
        {
            return _context.Posts.AsQueryable();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
            var postsCount = (pageNumber - 1) * pageSize;

            return await _context.Posts
                .Where(f => f.Title.ToLower().Contains(filterBy.ToLower()) || f.Content.ToLower().Contains(filterBy.ToLower()))
                .OrderByPropertyName(sortField,ascengind)
                .Skip(postsCount)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetAllCountAsync(string filterBy)
        {
            return await _context.Posts
                .Where(f => f.Title.ToLower().Contains(filterBy.ToLower()) || f.Content.ToLower().Contains(filterBy.ToLower()))
                .CountAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> AddAsync(Post post)
        {
            var createdPost = await _context.Posts.AddAsync(post);
            //koniecznie należy pamiętać o wywołaniu metody SaveChanges, aby zmiany zostały zapisane w bazie danych
            await _context.SaveChangesAsync();

            return createdPost.Entity;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        #endregion

    }
}
