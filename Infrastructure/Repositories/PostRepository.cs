using Domain.Entities;
using Infrastructure.Data;

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

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
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
        }

        #endregion

    }
}
