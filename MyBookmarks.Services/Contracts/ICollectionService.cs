using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Contracts
{
    /// <summary>
    /// Defines operations for managing a user's collections and their bookmarks.
    /// </summary>
    public interface ICollectionService
    {
        /// <summary>
        /// Retrieves a page of the user's collections, ordered by name and then by ID.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="pageNumber">The requested page number, starting at 1. Values outside the available range are adjusted to that range.</param>
        /// <returns>
        /// The collections and pagination information. The collection list is empty if the user has no collections.
        /// </returns>
        Task<PagedResultDto<CollectionListItemDto>>
            GetUserCollectionsAsync(string userId, int pageNumber);

        /// <summary>
        /// Creates a collection for the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="dto">The collection data, with the required name and field length limits already validated.</param>
        /// <returns>
        /// True if created; false if the user already has a collection with the same trimmed name.
        /// </returns>
        /// <remarks>
        /// The name and description are trimmed before saving.
        /// </remarks>
        Task<bool> TryCreateAsync(string userId, CollectionCreateDto dto);

        /// <summary>
        /// Retrieves the user's collection data for editing.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The collection data, or null if the collection does not exist or belongs to another user.
        /// </returns>
        Task<CollectionEditDto?> GetForEditAsync(string userId, int id);

        /// <summary>
        /// Updates a collection owned by the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="dto">The collection ID and updated data, with the required name and field length limits already validated.</param>
        /// <returns>
        /// Success if updated; NotFound if the collection is missing or belongs to another user;
        /// DuplicateName if another collection owned by the user has the same trimmed name.
        /// </returns>
        /// <remarks>
        /// The name and description are trimmed before saving.
        /// </remarks>
        Task<CollectionEditResult> EditAsync(string userId, CollectionEditDto dto);

        /// <summary>
        /// Retrieves the user's collection data for deletion confirmation.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The collection ID and name, or null if the collection does not exist or belongs to another user.
        /// </returns>
        Task<CollectionDeleteDto?> GetForDeleteAsync(string userId, int id);

        /// <summary>
        /// Deletes a collection owned by the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// True if deleted; false if the collection does not exist or belongs to another user.
        /// </returns>
        /// <remarks>
        /// The collection's bookmark links are removed; the bookmarks themselves are not deleted.
        /// </remarks>
        Task<bool> TryDeleteAsync(string userId, int id);

        /// <summary>
        /// Retrieves the user's collection details and a page of its bookmarks, ordered by title and then by ID.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <param name="pageNumber">The requested page number, starting at 1. Values outside the available range are adjusted to that range.</param>
        /// <returns>
        /// The collection details and bookmark pagination information, or null if the collection is missing or belongs to another user.
        /// An existing collection with no matching bookmarks has an empty bookmark list.
        /// </returns>
        Task<CollectionDetailsDto?> GetDetailsAsync(string userId, int id, int pageNumber);

        /// <summary>
        /// Adds a bookmark to a collection when both belong to the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user who must own both resources.</param>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <returns>
        /// True if the bookmark is added or is already in the collection;
        /// false if either resource is missing or belongs to another user.
        /// </returns>
        Task<bool> TryAddBookmarkAsync(string userId, int collectionId, int bookmarkId);

        /// <summary>
        /// Retrieves the ID, name and description of a collection owned by the specified user.
        /// </summary>
        /// <param name="userId">The collection owner's ID.</param>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The collection data without its bookmark list, or null if the collection is missing or belongs to another user.
        /// </returns>
        Task<CollectionListItemDto?> GetByIdAsync(string userId, int id);

        /// <summary>
        /// Identifies which of the supplied bookmarks are already included in the user's collection.
        /// </summary>
        /// <param name="userId">The ID of the user who must own both the collection and the bookmarks.</param>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkIds">The bookmark IDs to check.</param>
        /// <returns>
        /// The matching bookmark IDs, or an empty list if none match.
        /// </returns>
        Task<List<int>> GetIncludedBookmarkIdsAsync
            (string userId, int collectionId, List<int> bookmarkIds);

        /// <summary>
        /// Removes a bookmark from a collection when both belong to the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user who must own both resources.</param>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <returns>
        /// True if the link is removed or already absent;
        /// false if either resource is missing or belongs to another user.
        /// </returns>
        /// <remarks>
        /// The bookmark itself is not deleted.
        /// </remarks>
        Task<bool> TryRemoveBookmarkAsync(string userId, int collectionId, int bookmarkId);
    }
}
