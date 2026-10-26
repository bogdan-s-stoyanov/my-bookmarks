using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface IBookmarkRepository
    {
        Task<List<Bookmark>> GetBookmarksByUserIdAsync(
            string userId, int take, int skip, string? searchTerm = null);

        Task<int> CountBookmarksForUserAsync(string userId, string? searchTerm = null);

        Task AddAsync(Bookmark bookmark);

        Task<bool> ExistsByIdAsync(string userId, int id);

        Task<bool> ExistsByUrlAsync(string userId, string url);

        Task<bool> ExistsByTitleAsync(string userId, string title);

        Task<Bookmark?> GetByIdAsync(string userId, int id);

        Task UpdateAsync(Bookmark bookmark);

        Task<bool> ExistsByTitleExceptIdAsync(string userId, string title, int excludedId);

        Task<bool> ExistsByUrlExceptIdAsync(string userId, string url, int excludedId);

        Task DeleteAsync(Bookmark bookmark);

        Task<List<Bookmark>> GetByCollectionAsync(string userId, int collectionId, 
            int take, int skip);

        Task<int> CountByCollectionAsync(string userId, int collectionId);
    }
}
