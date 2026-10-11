using Microsoft.AspNetCore.Identity;

namespace MyBookmarks.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public IdentityUser User { get; set; } = null!;

        public ICollection<Bookmark> Bookmarks { get; set; }
            = new HashSet<Bookmark>();
    }
}
