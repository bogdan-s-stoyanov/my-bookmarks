using Microsoft.EntityFrameworkCore;
using MyBookmarks.Common.Enums;
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

        public async Task<int> CountBookmarksForUserAsync(
            string userId, string? searchTerm = null, 
            int? categoryId = null, bool onlyFavorites = false)
        {
            IQueryable<Bookmark> query = dbContext.Bookmarks
                .Where(b => b.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm));
            }

            if (onlyFavorites)
            {
                query = query.Where(b => b.IsFavorite);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<Bookmark>> GetBookmarksByUserIdAsync(
            string userId, int take, int skip,
            string? searchTerm = null, int? categoryId = null,
            bool onlyFavorites = false,
            BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending)
        {
            IQueryable<Bookmark> query = dbContext.Bookmarks
                .AsNoTracking()
                .Where(b => b.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm));
            }

            if (onlyFavorites)
            {
                query = query.Where(b => b.IsFavorite);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            switch (sortOrder)
            {
                case BookmarkSortOrder.Oldest:
                {
                    query = query
                        .OrderBy(b => b.DateAdded)
                        .ThenBy(b => b.Id);
                    break;
                }
                case BookmarkSortOrder.TitleAscending:
                {
                    query = query
                        .OrderBy(b => b.Title)
                        .ThenBy(b => b.Id);
                    break;
                }
                case BookmarkSortOrder.TitleDescending:
                {
                    query = query
                        .OrderByDescending(b => b.Title)
                        .ThenByDescending(b => b.Id);
                    break;
                }
                default:
                {
                    query = query
                        .OrderByDescending(b => b.DateAdded)
                        .ThenByDescending(b => b.Id);
                    break;
                }
            }

            return await query
                .Include(b => b.Category)
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

        public async Task<Bookmark?> GetByIdAsync(string userId, int id)
        {
            return await dbContext.Bookmarks
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId && b.Id == id);
        }

        public async Task UpdateAsync(Bookmark bookmark)
        {
            dbContext.Bookmarks.Update(bookmark);
            await dbContext.SaveChangesAsync();
        }

        public Task<bool> ExistsByTitleExceptIdAsync(string userId, string title, int excludedId)
        {
            return dbContext.Bookmarks
                .AnyAsync(b => b.UserId == userId && b.Title == title && b.Id != excludedId);
        }

        public Task<bool> ExistsByUrlExceptIdAsync(string userId, string url, int excludedId)
        {
            return dbContext.Bookmarks
                .AnyAsync(b => b.UserId == userId && b.Url == url && b.Id != excludedId);
        }

        public async Task DeleteAsync(Bookmark bookmark)
        {
            dbContext.Bookmarks.Remove(bookmark);
            await dbContext.SaveChangesAsync();
        }

        public Task<List<Bookmark>> GetByCollectionAsync(string userId, int collectionId, int take, int skip)
        {
            return dbContext.Bookmarks
                .AsNoTracking()
                .Where(b =>
                    b.UserId == userId &&
                    b.BookmarkCollections.Any(bc =>
                        bc.CollectionId == collectionId &&
                        bc.Collection.UserId == userId))
                .Include(b => b.Category)
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> CountByCollectionAsync(string userId, int collectionId)
        {
            return await dbContext.Bookmarks
                .CountAsync(b => b.UserId == userId &&
                    b.BookmarkCollections.Any(bc =>
                        bc.CollectionId == collectionId &&
                        bc.Collection.UserId == userId));
        }
    }
}
