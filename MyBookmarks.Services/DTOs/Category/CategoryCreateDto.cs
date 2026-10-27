using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Category
{
    /// <summary>
    /// Carries the data for creating a category.
    /// </summary>
    public class CategoryCreateDto
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
