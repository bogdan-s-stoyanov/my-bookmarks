
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface ICollectionRepository
    {
        Task<List<Collection>> GetCollectionsByUserIdAsync
            (string userId, int take, int skip);

        Task<int> CountCollectionsForUserAsync(string userId);
    }
}
