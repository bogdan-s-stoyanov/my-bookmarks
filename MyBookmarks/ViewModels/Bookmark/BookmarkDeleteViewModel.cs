using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Bookmark
{
    /// <summary>
    /// Contains the submitted identifier and display text for bookmark deletion confirmation.
    /// </summary>
    public class BookmarkDeleteViewModel
    {
        /// <summary>
        /// Gets or sets the bookmark identifier submitted for the operation.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bookmark title displayed for deletion confirmation.
        /// </summary>
        public string? Title { get; set; }
    }
}
