using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    public class BookmarkCollection
    {
        public int BookmarkId { get; set; }

        [ForeignKey(nameof(BookmarkId))]
        public Bookmark Bookmark { get; set; } = null!;

        public int CollectionId { get; set; }

        [ForeignKey(nameof(CollectionId))]
        public Collection Collection { get; set; } = null!;
    }
}
