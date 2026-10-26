
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface ICollectionRepository
    {
        Task<List<Collection>> GetCollectionsByUserIdAsync
            (string userId, int take, int skip);

        Task<int> CountCollectionsForUserAsync(string userId);

        Task<bool> ExistsByNameAsync(string userId, string name);

        Task AddAsync(Collection collection);

        Task<Collection?> GetByIdAsync(string userId, int id);

        Task<bool> ExistsByNameExceptIdAsync(string userId, string name, int excludedId);

        Task UpdateAsync(Collection collection);

        Task DeleteAsync(Collection collection);

        Task<bool> ContainsBookmarkAsync(int collectionId, int bookmarkId);

        Task AddBookmarkAsync(BookmarkCollection bookmarkCollection);

        Task<BookmarkCollection?> GetBookmarkCollectionAsync(int collectionId, int bookmarkId);

        Task RemoveBookmarkFromCollection(BookmarkCollection bookmarkCollection);

        Task<List<int>> GetIncludedBookmarkIdsAsync
            (string userId, int collectionId, List<int> bookmarksIds);
    }
}
