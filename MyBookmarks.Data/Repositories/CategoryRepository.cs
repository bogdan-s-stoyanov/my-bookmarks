using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyBookmarksDbContext dbContext;

        public CategoryRepository(MyBookmarksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public async Task<bool> ExistsByNameAsync(string userId, string name)
        {
            return await dbContext.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name);
        }

        public async Task AddAsync(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Category?> GetById(string userId, int id)
        {
            return await dbContext.Categories
                .FirstOrDefaultAsync(c => c.UserId == userId && id == c.Id);
        }

        public async Task<bool> ExistsByNameExceptId(string userId, string name, int excludeId)
        {
            return await dbContext.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != excludeId);
        }

        public async Task UpdateAsync(Category category)
        {
            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();    
        }
    }
}
