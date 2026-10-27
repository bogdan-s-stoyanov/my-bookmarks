using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    /// <summary>
    /// Contains the submitted identifier and display text for category deletion confirmation.
    /// </summary>
    public class CategoryDeleteViewModel
    {
        /// <summary>
        /// Gets or sets the category identifier submitted for the operation.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name displayed for deletion confirmation.
        /// </summary>
        public string? Name { get; set; }
    }
}
