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

        public async Task<List<Category>> GetCategoryByUserIdAsync(string userId, int take, int skip)
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
    }
}
