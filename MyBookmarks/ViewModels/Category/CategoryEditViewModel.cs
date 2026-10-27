using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    /// <summary>
    /// Contains the identifier, editable data and validation rules for the category edit form.
    /// </summary>
    public class CategoryEditViewModel
    {
        /// <summary>
        /// Gets or sets the category identifier submitted for the operation.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        [Required]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
