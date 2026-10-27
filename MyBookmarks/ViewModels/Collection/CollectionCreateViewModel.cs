using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the input and validation rules for the collection creation form.
    /// </summary>
    public class CollectionCreateViewModel
    {
        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional collection description.
        /// </summary>
        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
