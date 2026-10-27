using MyBookmarks.Common.Enums;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    /// <summary>
    /// Defines operations for managing a user's bookmarks.
    /// </summary>
    public interface IBookmarkService
    {
        /// <summary>
        /// Retrieves a page of the user's bookmarks matching the specified filters and sort order.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="pageNumber">The requested page number, starting at 1. Values outside the available range are adjusted to that range.</param>
        /// <param name="searchTerm">Text to find within titles. Leading and trailing whitespace is removed; an empty value applies no title filter.</param>
        /// <param name="categoryId">The category ID to filter by, or null to include all categories.</param>
        /// <param name="onlyFavorites">True to include only favorites; false to apply no favorites filter.</param>
        /// <param name="sortOrder">The ordering to apply before pagination. Defaults to title ascending.</param>
        /// <returns>
        /// The matching bookmarks and pagination information. The bookmark list is empty if none match.
        /// </returns>
        Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(
             string userId, int pageNumber,
             string? searchTerm = null, int? categoryId = null,
             bool onlyFavorites = false,
             BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending);

        /// <summary>
        /// Creates a bookmark for the specified user after checking its URL, duplicates and selected category.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="bookmarkDto">The bookmark data. Title presence and the title and description length limits must already be validated.</param>
        /// <returns>
        /// Success if created; InvalidUrl if the URL is not an absolute HTTP or HTTPS URL or exceeds the allowed length after normalization;
        /// DuplicateTitle or DuplicateUrl if the user already has a matching bookmark;
        /// InvalidCategory if the selected category is missing or belongs to another user.
        /// </returns>
        /// <remarks>
        /// The title and description are trimmed, and the URL is normalized before saving.
        /// New bookmarks are not favorites and receive the current UTC creation time.
        /// </remarks>
        Task<BookmarkCreateResult> CreateAsync(string userId, BookmarkCreateDto bookmarkDto);

        /// <summary>
        /// Retrieves the user's bookmark data for editing.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// The bookmark data, or null if the bookmark does not exist or belongs to another user.
        /// </returns>
        Task<BookmarkEditDto?> GetForEditAsync(string userId, int id);

        /// <summary>
        /// Updates a bookmark owned by the specified user after checking its URL, duplicates and selected category.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="bookmarkEditDto">The bookmark ID and updated data. Title presence and the title and description length limits must already be validated.</param>
        /// <returns>
        /// Success if updated; NotFound if the bookmark is missing or belongs to another user;
        /// InvalidUrl if the URL is not an absolute HTTP or HTTPS URL or exceeds the allowed length after normalization;
        /// DuplicateTitle or DuplicateUrl if another bookmark owned by the user matches;
        /// InvalidCategory if the selected category is missing or belongs to another user.
        /// </returns>
        /// <remarks>
        /// The title and description are trimmed, and the URL is normalized before saving.
        /// The creation time and favorite status are preserved.
        /// </remarks>
        Task<BookmarkEditResult> EditAsync(string userId, BookmarkEditDto bookmarkEditDto);

        /// <summary>
        /// Retrieves the user's bookmark data for deletion confirmation.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// The bookmark ID and title, or null if the bookmark does not exist or belongs to another user.
        /// </returns>
        Task<BookmarkDeleteDto?> GetForDeleteAsync(string userId, int id);

        /// <summary>
        /// Deletes a bookmark owned by the specified user.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// True if deleted; false if the bookmark does not exist or belongs to another user.
        /// </returns>
        Task<bool> TryDeleteAsync(string userId, int id);

        /// <summary>
        /// Reverses the favorite status of a bookmark owned by the specified user.
        /// </summary>
        /// <param name="userId">The bookmark owner's ID.</param>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// True if the status is updated; false if the bookmark does not exist or belongs to another user.
        /// </returns>
        /// <remarks>
        /// The return value indicates whether the update succeeded, not the resulting favorite status.
        /// </remarks>
        Task<bool> ToggleFavoriteAsync(string userId, int id);
    }
}
