using Microsoft.AspNetCore.Identity;
using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    /// <summary>
    /// Represents a user-owned category for classifying bookmarks.
    /// </summary>
    /// <remarks>
    /// A bookmark can have at most one category. Deleting a category leaves its bookmarks uncategorized.
    /// </remarks>
    public class Category
    {
        /// <summary>
        /// Gets or sets the database-generated category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name, which must be unique within the owner's categories.
        /// </summary>
        [Required]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Identity user ID of the category's owner.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the owner navigation.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the bookmarks assigned to this category.
        /// </summary>
        public ICollection<Bookmark> Bookmarks { get; set; }
            = new HashSet<Bookmark>();
    }
}
