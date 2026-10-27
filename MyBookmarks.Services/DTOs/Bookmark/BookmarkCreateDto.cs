using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    /// <summary>
    /// Carries the data for creating a bookmark.
    /// </summary>
    public class BookmarkCreateDto
    {
        /// <summary>
        /// Gets or sets the bookmark title.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the bookmark URL.
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional bookmark description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the selected category ID, or null to leave the bookmark uncategorized.
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
