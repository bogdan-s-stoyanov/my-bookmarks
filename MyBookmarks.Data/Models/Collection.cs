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
        /// <summary>
        /// Gets or sets the database-generated collection identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection name, which must be unique within the owner's collections.
        /// </summary>
        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional description of the collection.
        /// </summary>
        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the Identity user ID of the collection's owner.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the owner navigation.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the links to bookmarks included in this collection.
        /// </summary>
        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
           = new HashSet<BookmarkCollection>();
    }
}
