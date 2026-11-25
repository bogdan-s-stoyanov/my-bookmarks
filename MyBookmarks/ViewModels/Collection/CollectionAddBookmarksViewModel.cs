using MyBookmarks.ViewModels.Bookmark;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionAddBookmarksViewModel
    {
        public int CollectionId { get; set; }

        public string CollectionName { get; set; } = null!;

        public BookmarkIndexViewModel BookmarksPage { get; set; } = new();

        /// <summary>
        /// IDs of bookmarks on the current page already included in the collection.
        /// </summary>
        public List<int> IncludedBookmarkIds { get; set; } = new();
    }
}
