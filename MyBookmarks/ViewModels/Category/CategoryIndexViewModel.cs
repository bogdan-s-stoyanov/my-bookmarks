namespace MyBookmarks.ViewModels.Category
{
    /// <summary>
    /// Contains the current category list page and pagination information.
    /// </summary>
    public class CategoryIndexViewModel
    {
        /// <summary>
        /// Contains the category items displayed on the current page.
        /// </summary>
        public List<CategoryListItemViewModel> Categories = new();

        /// <summary>
        /// Gets or sets the total number of matching items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number, starting at 1.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the number of pages required for the total item count.
        /// </summary>
        /// <remarks>
        /// PageSize must be positive. An empty result has zero pages.
        /// </remarks>
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
