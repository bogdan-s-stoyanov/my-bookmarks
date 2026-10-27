using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    /// <summary>
    /// Carries the identifier and display text for collection deletion confirmation.
    /// </summary>
    public class CollectionDeleteDto
    {
        /// <summary>
        /// Gets or sets the collection identifier submitted for the operation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection name displayed for deletion confirmation.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}