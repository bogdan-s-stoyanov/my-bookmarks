using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the submitted identifier and display text for collection deletion confirmation.
    /// </summary>
    public class CollectionDeleteViewModel
    {
        /// <summary>
        /// Gets or sets the collection identifier submitted for the operation.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection name displayed for deletion confirmation.
        /// </summary>
        public string? Name { get; set; }
    }
}
