using MyBookmarks.Common.Enums;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    /// <summary>
    /// Defines database operations for managing bookmarks.
    /// </summary>
    public interface IBookmarkRepository
    {
        /// <summary>
        /// Retrieves a page of the user's bookmarks matching the specified filters and sort order.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="take">The maximum number of bookmarks to return.</param>
        /// <param name="skip">The number of bookmarks to skip.</param>
        /// <param name="searchTerm">Text to find within titles. Null, empty or whitespace applies no title filter.</param>
        /// <param name="categoryId">The category ID to filter by, or null to include all categories.</param>
        /// <param name="onlyFavorites">True to include only favorites; false to apply no favorites filter.</param>
        /// <param name="sortOrder">The ordering to apply before pagination. Defaults to title ascending.</param>
        /// <returns>
        /// The matching bookmarks with their categories, or an empty list if none match.
        /// </returns>
        Task<List<Bookmark>> GetBookmarksByUserIdAsync(
            string userId, int take, int skip,
            string? searchTerm = null, int? categoryId = null,
            bool onlyFavorites = false,
            BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending);

        /// <summary>
        /// Counts the user's bookmarks matching the specified filters.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="searchTerm">Text to find within titles. Null, empty or whitespace applies no title filter.</param>
        /// <param name="categoryId">The category ID to filter by, or null to include all categories.</param>
        /// <param name="onlyFavorites">True to include only favorites; false to apply no favorites filter.</param>
        /// <returns>
        /// The total number of matching bookmarks before pagination.
        /// </returns>
        Task<int> CountBookmarksForUserAsync(
            string userId, string? searchTerm = null,
            int? categoryId = null, bool onlyFavorites = false);

        /// <summary>
        /// Adds a bookmark and saves the changes to the database.
        /// </summary>
        /// <param name="bookmark">The bookmark to add.</param>
        Task AddAsync(Bookmark bookmark);

        /// <summary>
        /// Checks whether a bookmark with the specified ID belongs to the user.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// True if the user owns the bookmark; otherwise, false.
        /// </returns>
        Task<bool> ExistsByIdAsync(string userId, int id);

        /// <summary>
        /// Checks whether the user has a bookmark with the specified URL.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="url">The URL to compare with stored bookmark URLs.</param>
        /// <returns>
        /// True if a matching bookmark exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByUrlAsync(string userId, string url);

        /// <summary>
        /// Checks whether the user has a bookmark with the specified title.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="title">The bookmark title to check.</param>
        /// <returns>
        /// True if a matching bookmark exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByTitleAsync(string userId, string title);

        /// <summary>
        /// Retrieves a bookmark by ID that belongs to the specified user.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// The bookmark, or null if it does not exist or belongs to another user.
        /// </returns>
        Task<Bookmark?> GetByIdAsync(string userId, int id);

        /// <summary>
        /// Updates a bookmark and saves the changes to the database.
        /// </summary>
        /// <param name="bookmark">The bookmark containing the updated data.</param>
        Task UpdateAsync(Bookmark bookmark);

        /// <summary>
        /// Checks whether the user has another bookmark with the specified title.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="title">The bookmark title to check.</param>
        /// <param name="excludedId">The ID of the bookmark to exclude from the check.</param>
        /// <returns>
        /// True if another matching bookmark exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByTitleExceptIdAsync(string userId, string title, int excludedId);

        /// <summary>
        /// Checks whether the user has another bookmark with the specified URL.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="url">The URL to compare with stored bookmark URLs.</param>
        /// <param name="excludedId">The ID of the bookmark to exclude from the check.</param>
        /// <returns>
        /// True if another matching bookmark exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByUrlExceptIdAsync(string userId, string url, int excludedId);

        /// <summary>
        /// Deletes a bookmark and saves the changes to the database.
        /// </summary>
        /// <param name="bookmark">The bookmark to delete.</param>
        Task DeleteAsync(Bookmark bookmark);

        /// <summary>
        /// Retrieves a page of the user's bookmarks in the specified collection, ordered by title and then by ID.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="collectionId">The collection ID. The collection must also belong to the user.</param>
        /// <param name="take">The maximum number of bookmarks to return.</param>
        /// <param name="skip">The number of bookmarks to skip.</param>
        /// <returns>
        /// The matching bookmarks with their categories, or an empty list if none match.
        /// </returns>
        Task<List<Bookmark>> GetByCollectionAsync(string userId, int collectionId, 
            int take, int skip);

        /// <summary>
        /// Counts the user's bookmarks in the specified collection.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="collectionId">The collection ID. The collection must also belong to the user.</param>
        /// <returns>
        /// The number of matching bookmarks, or zero if the collection is missing, belongs to another user or contains no matching bookmarks.
        /// </returns>
        Task<int> CountByCollectionAsync(string userId, int collectionId);

    }
}
