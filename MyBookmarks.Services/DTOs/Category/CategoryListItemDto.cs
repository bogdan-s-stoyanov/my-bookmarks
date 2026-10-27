using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Category
{
    /// <summary>
    /// Carries the data for one category in a list.
    /// </summary>
    public class CategoryListItemDto
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
