using MyBookmarks.ViewModels.Bookmark;

namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the target collection, a page of selectable bookmarks and their existing memberships.
    /// </summary>
    public class CollectionAddBookmarksViewModel
    {
        /// <summary>
        /// Gets or sets the target collection identifier.
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// Gets or sets the target collection name displayed on the selection page.
        /// </summary>
        public string CollectionName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a page of the user's bookmarks available for selection.
        /// </summary>
        public BookmarkIndexViewModel BookmarksPage { get; set; } = new();

        /// <summary>
        /// Gets or sets the IDs on the displayed page that already belong to the target collection.
        /// </summary>
        public List<int> IncludedBookmarkIds { get; set; } = new();
    }
}
