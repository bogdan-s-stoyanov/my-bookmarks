using Microsoft.AspNetCore.Identity;

namespace MyBookmarks.Data.Models
{
    public class Bookmark
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsFavorite { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public IdentityUser User { get; set; } = null!;

        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
            = new HashSet<BookmarkCollection>();
    }
}
