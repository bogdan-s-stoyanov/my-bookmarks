using MyBookmarks.ViewModels.Bookmark;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionAddBookmarksViewModel
    {
        public int CollectionId { get; set; }

        public string CollectionName { get; set; } = null!;

        public BookmarkIndexViewModel BookmarksPage { get; set; } = new();

        public List<int> IncludedBookmarkIds { get; set; } = new();
    }
}
