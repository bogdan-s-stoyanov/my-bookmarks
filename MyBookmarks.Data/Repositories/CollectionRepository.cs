using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories
{
    /// <summary>
    /// Reads and persists collection data using Entity Framework Core.
    /// </summary>
    public class CollectionRepository : ICollectionRepository
    {
        private MyBookmarksDbContext dbContext;

        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="dbContext">The database context used for collection storage.</param>
        public CollectionRepository(MyBookmarksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task<int> CountCollectionsForUserAsync(string userId)
        {
            return dbContext.Collections
                .CountAsync(c => c.UserId == userId);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task AddAsync(Collection collection)
        {
            dbContext.Collections.Add(collection);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameAsync(string userId, string name)
        {
            return await dbContext.Collections
                .AnyAsync(c => c.UserId == userId && c.Name == name);
        }

        /// <inheritdoc />
        public async Task<Collection?> GetByIdAsync(string userId, int id)
        {
            return await dbContext.Collections
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == id);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameExceptIdAsync(string userId, string name, int excludedId)
        {
            return await dbContext.Collections
                .AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != excludedId);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Collection collection)
        {
            dbContext.Collections.Update(collection);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Collection collection)
        {
            dbContext.Collections.Remove(collection);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ContainsBookmarkAsync(int collectionId, int bookmarkId)
        {
            return await dbContext.BookmarkCollections
                .AnyAsync(bc => bc.CollectionId == collectionId &&
                    bc.BookmarkId == bookmarkId);
        }

        /// <inheritdoc />
        public async Task AddBookmarkAsync(BookmarkCollection bookmarkCollection)
        {
            dbContext.BookmarkCollections.Add(bookmarkCollection);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<BookmarkCollection?> GetBookmarkCollectionAsync(int collectionId, int bookmarkId)
        {
            return await dbContext.BookmarkCollections
                .FirstOrDefaultAsync(bc =>
                    bc.CollectionId == collectionId &&
                    bc.BookmarkId == bookmarkId);
        }

        /// <inheritdoc />
        public async Task RemoveBookmarkFromCollection(BookmarkCollection bookmarkCollection)
        {
            dbContext.BookmarkCollections.Remove(bookmarkCollection);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<int>> GetIncludedBookmarkIdsAsync(string userId, int collectionId, List<int> bookmarksIds)
        {
            if (bookmarksIds.Count == 0)
            {
                return new List<int>();
            }

            return await dbContext.BookmarkCollections
                .Where(bc =>
                    bc.CollectionId == collectionId &&
                    bc.Collection.UserId == userId &&
                    bc.Bookmark.UserId == userId &&
                    bookmarksIds.Contains(bc.BookmarkId))
                .OrderBy(bc => bc.Bookmark.Title)
                .Take(bookmarksIds.Count)
                .Select(bc => bc.BookmarkId)
                .ToListAsync();
        }
    }
}
