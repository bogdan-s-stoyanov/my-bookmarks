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
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public ICollection<Bookmark> Bookmarks { get; set; }
            = new HashSet<Bookmark>();
    }
}
