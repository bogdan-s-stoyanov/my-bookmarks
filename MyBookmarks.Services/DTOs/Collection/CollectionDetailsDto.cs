using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    /// <summary>
    /// Carries collection details and a page of its bookmarks.
    /// </summary>
    public class CollectionDetailsDto
    {
        /// <summary>
        /// Gets or sets the collection identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional collection description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the current page of bookmarks in the collection and its pagination information.
        /// </summary>
        public PagedResultDto<BookmarkListItemDto> BookmarksPage { get; set; }
            = new();
    }
}
