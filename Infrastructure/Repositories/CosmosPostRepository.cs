﻿using Cosmonaut;
using Domain.Entities.Cosmos;

namespace Infrastructure.Repositories
{
    public class CosmosPostRepository : ICosmosPostRepository
    {
        private readonly ICosmosStore<CosmosPost> _cosmosStore;

        #region Contructor

        public CosmosPostRepository(/*pozwalana na pobieranie i manipulowanie danymi w bazie azure cosmos db*/ICosmosStore<CosmosPost> cosmosStore)
        {
            _cosmosStore =
                cosmosStore ?? throw new ArgumentNullException(nameof(cosmosStore));
        }

        #endregion

        #region ICosmosPostRepository

        public async Task<IEnumerable<CosmosPost>> GetAllAsync()
        {
            var posts = await _cosmosStore.Query().ToListAsync();
            return posts;
        }

        public async Task<CosmosPost> GetByIdAsync(string id)
        {
            return await _cosmosStore.FindAsync(id);
        }

        public async Task<CosmosPost> AddAsync(CosmosPost post)
        {
            post.Id = Guid.NewGuid().ToString();
            return await _cosmosStore.AddAsync(post);
        }

        public async Task UpdateAsync(CosmosPost post)
        {
            await _cosmosStore.UpdateAsync(post);
        }

        public async Task DeleteAsync(CosmosPost post)
        {
            await _cosmosStore.RemoveAsync(post);
        }

        #endregion

    }
}
