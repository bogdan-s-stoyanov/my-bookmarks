using Microsoft.AspNetCore.Identity;
using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    /// <summary>
    /// Represents a user-owned group of bookmarks that can also appear in other collections.
    /// </summary>
    /// <remarks>
    /// Deleting a collection removes its bookmark links without deleting the bookmarks.
    /// </remarks>
    public class Collection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
           = new HashSet<BookmarkCollection>();
    }
}
