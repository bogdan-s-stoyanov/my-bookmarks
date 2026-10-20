using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        [Required]
        [StringLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
