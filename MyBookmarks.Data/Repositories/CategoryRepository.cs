using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories
{
    /// <summary>
    /// Reads and persists category data using Entity Framework Core.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyBookmarksDbContext dbContext;

        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="dbContext">The database context used for category storage.</param>
        public CategoryRepository(MyBookmarksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<List<Category>> GetCategoriesByUserIdAsync(string userId, int take, int skip)
        {
            return await dbContext.Categories
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameAsync(string userId, string name)
        {
            return await dbContext.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name);
        }

        /// <inheritdoc />
        public async Task AddAsync(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Category?> GetById(string userId, int id)
        {
            return await dbContext.Categories
                .FirstOrDefaultAsync(c => c.UserId == userId && id == c.Id);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameExceptId(string userId, string name, int excludeId)
        {
            return await dbContext.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != excludeId);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Category category)
        {
            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Category category)
        {
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();    
        }

        /// <inheritdoc />
        public async Task<int> CountCategoriesForUserAsync(string userId)
        {
            return await dbContext.Categories
                .CountAsync(c => c.UserId == userId);
        }
    }
}
