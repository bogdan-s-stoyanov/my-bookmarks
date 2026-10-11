namespace MyBookmarks.Data.Models
{
    public class BookmarkCollection
    {
        public int BookmarkId { get; set; }

        public Bookmark Bookmark { get; set; } = null!;

        public int CollectionId { get; set; }

        public Collection Collection { get; set; } = null!;
    }
}
