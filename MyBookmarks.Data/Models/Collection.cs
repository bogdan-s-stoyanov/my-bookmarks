using Microsoft.AspNetCore.Identity;

namespace MyBookmarks.Data.Models
{
    public class Collection
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string UserId { get; set; } = null!;

        public IdentityUser User { get; set; } = null!;

        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
           = new HashSet<BookmarkCollection>();
    }
}
