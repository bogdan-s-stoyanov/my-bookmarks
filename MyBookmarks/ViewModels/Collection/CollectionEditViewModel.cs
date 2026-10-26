using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionEditViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
