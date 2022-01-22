using Dapper;
using Domain.Entities;
using Infrastructure.Data.Dapper;

namespace Infrastructure.Repositories
{
    public class DapperPostRepository : IDapperPostRepository
    {
        private readonly DapperContext _context;

        #region Contructor

        public DapperPostRepository(DapperContext context)
        {
            _context =
                context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region IDapperRepository
        public async Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy)
        {
            var postsCount = (pageNumber - 1) * pageSize;

            var sortType = ascengind ? "asc" : "desc";

            var query = "SELECT Id" +
                        "   ,Title" +
                        "   ,Content" +
                        "   ,Created" +
                        "   ,CreatedBy" +
                        "   ,LastModified" +
                        "   ,LastModifiedBy" +
                        "FROM dbo.Posts" +
                       $"WHERE Title LIKE '%{filterBy}%' OR Content LIKE '%{filterBy}%'" +
                       $"ORDER BY {sortField} {sortType}" +
                       $"OFFSET {postsCount} ROWS" +
                       $"FETCH NEXT {pageSize} ROWS ONLY";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryAsync<Post>(query);
                return posts.ToList();
            }
        }

        public async Task<int> GetAllCountAsync(string filterBy)
        {
            var query = "SELECT COUNT(*)" +
                        "FROM dbo.Posts" +
                       $"WHERE Title LIKE '%{filterBy}%' OR Content LIKE '%{filterBy}%'";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryFirstOrDefaultAsync<int>(query);
                return posts;
            }
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var query = "SELECT Id" +
                        "   ,Title" +
                        "   ,Content" +
                        "   ,Created" +
                        "   ,CreatedBy" +
                        "   ,LastModified" +
                        "   ,LastModifiedBy" +
                        "FROM dbo.Posts" +
                       $"WHERE Id = {id}";

            using (var connection = _context.CreateConnection())
            {
                var post = await connection.QueryFirstOrDefaultAsync<Post>(query);
                return post;
            }
        }

        public async Task<Post> AddAsync(Post post)
        {

            string query = "INSERT INTO dbo.Posts (Title,Content,Created)" +
                          $"VALUES ({post.Title},{post.Content},{post.Created})" +
                           "SELECT CONVERT(INT,SCOPE_IDENTITY())";

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QueryFirstOrDefaultAsync<int>(query);
                return new Post { Id = id, Title = post.Title, Content = post.Content, Created = post.Created };
            }
        }

        public async Task UpdateAsync(Post post)
        {
            var query = "UPDATE dbo.Posts " +
                       $"SET Id = {post.Id}" +
                       $"   ,Title = {post.Title}" +
                       $"   ,Content = {post.Content}" +
                       $"   ,Created = {post.Created}" +
                       $"   ,CreatedBy = {post.CreatedBy}" +
                       $"   ,LastModified = {post.LastModified}" +
                       $"   ,LastModifiedBy = {post.LastModifiedBy}" +
                        "FROM dbo.Posts" +
                       $"WHERE Id = {post.Id}";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query);
            }
        }

        public async Task DeleteAsync(Post post)
        {
            var query = "DELETE" +
                        "FROM dbo.Posts " +
                       $"WHERE Id = {post.Id}";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query);
            }
        }

        #endregion

    }
}
