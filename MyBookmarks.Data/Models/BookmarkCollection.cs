using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    /// <summary>
    /// Represents one link in the many-to-many relationship between bookmarks and collections.
    /// </summary>
    /// <remarks>
    /// The bookmark and collection identifiers form the composite primary key.
    /// </remarks>
    public class BookmarkCollection
    {
        /// <summary>
        /// Gets or sets the identifier of the linked bookmark.
        /// </summary>
        public int BookmarkId { get; set; }

        /// <summary>
        /// Gets or sets the linked bookmark navigation.
        /// </summary>
        [ForeignKey(nameof(BookmarkId))]
        public Bookmark Bookmark { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the linked collection.
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// Gets or sets the linked collection navigation.
        /// </summary>
        [ForeignKey(nameof(CollectionId))]
        public Collection Collection { get; set; } = null!;
    }
}
