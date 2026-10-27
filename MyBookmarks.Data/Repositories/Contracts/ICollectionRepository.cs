
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data.Repositories.Contracts
{
    /// <summary>
    /// Defines database operations for managing collections and their bookmark links.
    /// </summary>
    public interface ICollectionRepository
    {
        /// <summary>
        /// Retrieves a page of the user's collections, ordered by name and then by ID.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="take">The maximum number of collections to return.</param>
        /// <param name="skip">The number of collections to skip.</param>
        /// <returns>
        /// The requested collections, or an empty list if none match.
        /// </returns>
        Task<List<Collection>> GetCollectionsByUserIdAsync
            (string userId, int take, int skip);

        /// <summary>
        /// Counts the collections owned by the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <returns>
        /// The number of collections owned by the user.
        /// </returns>
        Task<int> CountCollectionsForUserAsync(string userId);

        /// <summary>
        /// Checks whether the user has a collection with the specified name.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="name">The collection name to check.</param>
        /// <returns>
        /// True if a matching collection exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByNameAsync(string userId, string name);

        /// <summary>
        /// Adds a collection and saves the changes to the database.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        Task AddAsync(Collection collection);

        /// <summary>
        /// Retrieves a collection by ID that belongs to the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The collection, or null if it does not exist or belongs to another user.
        /// </returns>
        Task<Collection?> GetByIdAsync(string userId, int id);

        /// <summary>
        /// Checks whether the user has another collection with the specified name.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="name">The collection name to check.</param>
        /// <param name="excludedId">The ID of the collection to exclude from the check.</param>
        /// <returns>
        /// True if another matching collection exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByNameExceptIdAsync(string userId, string name, int excludedId);

        /// <summary>
        /// Updates a collection and saves the changes to the database.
        /// </summary>
        /// <param name="collection">The collection containing the updated data.</param>
        Task UpdateAsync(Collection collection);

        /// <summary>
        /// Deletes a collection and saves the changes to the database.
        /// </summary>
        /// <param name="collection">The collection to delete.</param>
        /// <remarks>
        /// The collection's bookmark links are removed; the bookmarks themselves are not deleted.
        /// </remarks>
        Task DeleteAsync(Collection collection);

        /// <summary>
        /// Checks whether a bookmark is linked to the specified collection.
        /// </summary>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <returns>
        /// True if the link exists; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method does not check ownership. The caller must verify that the user owns both the collection and the bookmark.
        /// </remarks>
        Task<bool> ContainsBookmarkAsync(int collectionId, int bookmarkId);

        /// <summary>
        /// Adds a bookmark-to-collection link and saves the changes to the database.
        /// </summary>
        /// <param name="bookmarkCollection">The link between an existing bookmark and collection.</param>
        /// <remarks>
        /// The caller must verify ownership of both resources and check that the link does not already exist.
        /// </remarks>
        Task AddBookmarkAsync(BookmarkCollection bookmarkCollection);

        /// <summary>
        /// Retrieves the link between a bookmark and a collection.
        /// </summary>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <returns>
        /// The link, or null if the bookmark is not linked to the collection.
        /// </returns>
        /// <remarks>
        /// This method does not check ownership. The caller must verify that the user owns both the collection and the bookmark.
        /// </remarks>
        Task<BookmarkCollection?> GetBookmarkCollectionAsync(int collectionId, int bookmarkId);

        /// <summary>
        /// Removes a bookmark-to-collection link and saves the changes to the database.
        /// </summary>
        /// <param name="bookmarkCollection">The link to remove.</param>
        /// <remarks>
        /// The bookmark and collection themselves are not deleted.
        /// The caller must verify ownership of both resources.
        /// </remarks>
        Task RemoveBookmarkFromCollection(BookmarkCollection bookmarkCollection);

        /// <summary>
        /// Identifies which of the supplied bookmarks are already linked to the user's collection.
        /// </summary>
        /// <param name="userId">The ID of the user who must own both the collection and the bookmarks.</param>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarksIds">The bookmark IDs to check.</param>
        /// <returns>
        /// The matching bookmark IDs, or an empty list if none match.
        /// </returns>
        Task<List<int>> GetIncludedBookmarkIdsAsync
            (string userId, int collectionId, List<int> bookmarksIds);
    }
}
