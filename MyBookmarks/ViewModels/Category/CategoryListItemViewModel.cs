using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    /// <summary>
    /// Contains the data displayed for one category in a list.
    /// </summary>
    public class CategoryListItemViewModel
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
