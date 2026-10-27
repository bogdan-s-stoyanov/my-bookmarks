using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common.Enums;
using MyBookmarks.ViewModels.Category;

namespace MyBookmarks.ViewModels.Bookmark
{
    /// <summary>
    /// Contains the current bookmark list page, selected search and filter values and pagination information.
    /// </summary>
    public class BookmarkIndexViewModel
    {
        /// <summary>
        /// Contains the bookmark items displayed on the current page.
        /// </summary>
        public List<BookmarkListItemViewModel> Bookmarks { get; set; } = new();

        /// <summary>
        /// Gets or sets the current page number, starting at 1.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the total number of matching items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the number of pages required for the total item count.
        /// </summary>
        /// <remarks>
        /// PageSize must be positive. An empty result has zero pages.
        /// </remarks>
        public int TotalPages
            => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Gets or sets the text used to search within bookmark titles.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the category filter, or null to include all categories.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the current user's category options for filtering.
        /// </summary>
        public List<CategoryListItemViewModel> Categories { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the list is restricted to favorite bookmarks.
        /// </summary>
        public bool OnlyFavorites { get; set; }

        /// <summary>
        /// Gets or sets the bookmark ordering to retain in forms and page links.
        /// </summary>
        public BookmarkSortOrder SortOrder { get; set; } = BookmarkSortOrder.TitleAscending;
    }
}
