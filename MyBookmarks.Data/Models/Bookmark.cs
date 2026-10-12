using Microsoft.AspNetCore.Identity;
using MyBookmarks.Data.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    public class Bookmark
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(BookmarkValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(BookmarkValidationConstants.UrlMaxLength)]
        public string Url { get; set; } = null!;

        [MaxLength(BookmarkValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsFavorite { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
            = new HashSet<BookmarkCollection>();
    }
}
