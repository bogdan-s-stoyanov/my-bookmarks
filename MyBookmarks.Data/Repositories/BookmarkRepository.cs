using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private MyBookmarksDbContext dbContext;

        public BookmarkRepository(MyBookmarksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CountBookmarksForUserAsync(string userId)
        {
            return await dbContext.Bookmarks
                .CountAsync(c => c.UserId == userId);
        }

        public async Task<List<Bookmark>> GetBookmarksByUserIdAsync(string userId, int take, int skip)
        {
            return await dbContext.Bookmarks
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .Include(b => b.Category)
                .OrderByDescending(b => b.DateAdded)
                .ThenByDescending(b => b.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task AddAsync(Bookmark bookmark)
        {
            dbContext.Bookmarks.Add(bookmark);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(string userId, int id)
        {
            return await dbContext.Bookmarks
                .AnyAsync(b => b.UserId == userId && b.Id == id);
        }

        public async Task<bool> ExistsByUrlAsync(string userId, string url)
        {
            return await dbContext.Bookmarks
                .AnyAsync(b => b.UserId == userId && b.Url == url);
        }

        public async Task<bool> ExistsByTitleAsync(string userId, string title)
        {
            return await dbContext.Bookmarks
                .AnyAsync(b => b.UserId == userId && b.Title == title );
        }
    }
}
