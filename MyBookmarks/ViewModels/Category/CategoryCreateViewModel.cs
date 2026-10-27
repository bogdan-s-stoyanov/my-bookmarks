using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    /// <summary>
    /// Contains the input and validation rules for the category creation form.
    /// </summary>
    public class CategoryCreateViewModel
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        [Required]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
