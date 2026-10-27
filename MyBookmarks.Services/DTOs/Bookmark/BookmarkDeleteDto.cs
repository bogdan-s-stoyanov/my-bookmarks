using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    /// <summary>
    /// Carries the identifier and display text for bookmark deletion confirmation.
    /// </summary>
    public class BookmarkDeleteDto 
    {
        /// <summary>
        /// Gets or sets the bookmark identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bookmark title displayed for deletion confirmation.
        /// </summary>
        public string Title { get; set; } = null!;
    }
}
