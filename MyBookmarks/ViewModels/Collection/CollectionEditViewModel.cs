using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the identifier, editable data and validation rules for the collection edit form.
    /// </summary>
    public class CollectionEditViewModel
    {
        /// <summary>
        /// Gets or sets the collection identifier submitted for the operation.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

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
