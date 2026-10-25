using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private MyBookmarksDbContext dbContext;

        public CollectionRepository(MyBookmarksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<int> CountCollectionsForUserAsync(string userId)
        {
            return dbContext.Collections
                .CountAsync(c => c.UserId == userId);
        }

        public Task<List<Collection>> GetCollectionsByUserIdAsync(string userId, int take, int skip)
        {
            return dbContext.Collections
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task AddAsync(Collection collection)
        {
            dbContext.Collections.Add(collection);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string userId, string name)
        {
            return await dbContext.Collections
                .AnyAsync(c => c.UserId == userId && c.Name == name);
        }
    }
}
