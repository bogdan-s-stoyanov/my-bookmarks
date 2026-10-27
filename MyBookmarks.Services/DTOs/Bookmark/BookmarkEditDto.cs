using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    /// <summary>
    /// Carries a bookmark's editable data and identifier.
    /// </summary>
    public class BookmarkEditDto
    {
        /// <summary>
        /// Gets or sets the bookmark identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bookmark title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the bookmark URL.
        /// </summary>
        public string Url { get; set; }

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
