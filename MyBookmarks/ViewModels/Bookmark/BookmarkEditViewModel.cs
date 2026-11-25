using MyBookmarks.Common.EntityValidation;
using MyBookmarks.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Bookmark
{
    public class BookmarkEditViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(BookmarkValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [Url]
        [MaxLength(BookmarkValidationConstants.UrlMaxLength)]
        public string Url { get; set; } = null!;

        [MaxLength(BookmarkValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// The user's category options for the dropdown.
        /// </summary>
        public List<CategoryListItemViewModel> Categories { get; set; }
            = new();
    }
}
