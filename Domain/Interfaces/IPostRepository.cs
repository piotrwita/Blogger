using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Obrabia dane sortowane po stronie bazy
        /// </summary>
        /// <returns></returns>
        IQueryable<Post> GetAll();
        /// <summary>
        /// Pobiera wszystkie dane ze bazy i obrabia po stronie apliacji
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="ascengind"></param>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascengind, string filterBy);
        Task<int> GetAllCountAsync(string filterBy);
        Task<Post> GetByIdAsync(int id);
        Task<Post> AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
    }
}
