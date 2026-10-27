using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    /// <summary>
    /// Carries the data for one bookmark in a list.
    /// </summary>
    public class BookmarkListItemDto
    {
        /// <summary>
        /// Gets or sets the bookmark identifier.
        /// </summary>
        public int Id { get; set; }

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
        /// Gets or sets the category name, or null when the bookmark is uncategorized.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Gets or sets whether the owner has marked the bookmark as a favorite.
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
