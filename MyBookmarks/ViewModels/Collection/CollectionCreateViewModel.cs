using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionCreateViewModel
    {
        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
