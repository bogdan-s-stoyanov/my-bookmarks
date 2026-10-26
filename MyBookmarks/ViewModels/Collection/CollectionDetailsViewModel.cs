using MyBookmarks.ViewModels.Bookmark;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public BookmarkIndexViewModel BookmarksPage { get; set; } = new();
    }
}
