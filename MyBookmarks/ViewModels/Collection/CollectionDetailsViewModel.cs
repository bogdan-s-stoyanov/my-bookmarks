using MyBookmarks.ViewModels.Bookmark;

namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the collection details and current bookmark page displayed in its view.
    /// </summary>
    public class CollectionDetailsViewModel
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
        public BookmarkIndexViewModel BookmarksPage { get; set; } = new();
    }
}
