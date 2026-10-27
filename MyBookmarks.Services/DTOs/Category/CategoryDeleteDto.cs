using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Category
{
    /// <summary>
    /// Carries the identifier and display text for category deletion confirmation.
    /// </summary>
    public class CategoryDeleteDto
    {
        /// <summary>
        /// Gets or sets the category identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name displayed for deletion confirmation.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
