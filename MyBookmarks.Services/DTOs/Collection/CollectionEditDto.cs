using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    /// <summary>
    /// Carries a collection's editable data and identifier.
    /// </summary>
    public class CollectionEditDto
    {
        /// <summary>
        /// Gets or sets the collection identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

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
