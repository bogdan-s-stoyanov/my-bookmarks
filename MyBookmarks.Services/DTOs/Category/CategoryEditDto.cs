using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Category
{
    /// <summary>
    /// Carries a category's editable data and identifier.
    /// </summary>
    public class CategoryEditDto
    {
        /// <summary>
        /// Gets or sets the category identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
