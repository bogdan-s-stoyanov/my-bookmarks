using Microsoft.AspNetCore.Identity;
using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    /// <summary>
    /// Represents a web link saved by a user, optionally assigned to a category and any number of collections.
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// Gets or sets the database-generated bookmark identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title displayed for the saved link.
        /// </summary>
        [Required]
        [MaxLength(BookmarkValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the saved absolute HTTP or HTTPS URL.
        /// </summary>
        [Required]
        [MaxLength(BookmarkValidationConstants.UrlMaxLength)]
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional notes describing the bookmark.
        /// </summary>
        [MaxLength(BookmarkValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp recording when the bookmark was added.
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Gets or sets whether the owner has marked the bookmark as a favorite.
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Gets or sets the category identifier, or null when the bookmark is uncategorized.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the optional category navigation.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Identity user ID of the bookmark's owner.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the owner navigation.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the links to collections containing this bookmark.
        /// </summary>
        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
            = new HashSet<BookmarkCollection>();
    }
}
