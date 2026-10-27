using MyBookmarks.Common.EntityValidation;
using MyBookmarks.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Bookmark
{
    /// <summary>
    /// Contains the input and validation rules for the bookmark creation form.
    /// </summary>
    public class BookmarkCreateViewModel
    {
        /// <summary>
        /// Gets or sets the bookmark title.
        /// </summary>
        [Required]
        [MaxLength(BookmarkValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the bookmark URL.
        /// </summary>
        [Required]
        [Url]
        [MaxLength(BookmarkValidationConstants.UrlMaxLength)]
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional bookmark description.
        /// </summary>
        [MaxLength(BookmarkValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the selected category ID, or null to leave the bookmark uncategorized.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the current user's category options for the selection control.
        /// </summary>
        public List<CategoryListItemViewModel> Categories { get; set; }
            = new();
    }
}
