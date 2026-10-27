using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    /// <summary>
    /// Carries the data for creating a collection.
    /// </summary>
    public class CollectionCreateDto
    {
        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional collection description.
        /// </summary>
        public string? Description { get; set; }
    }
}
