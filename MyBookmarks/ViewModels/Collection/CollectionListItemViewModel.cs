namespace MyBookmarks.ViewModels.Collection
{
    /// <summary>
    /// Contains the data displayed for one collection in a list.
    /// </summary>
    public class CollectionListItemViewModel
    {
        /// <summary>
        /// Gets or sets the collection identifier.
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
