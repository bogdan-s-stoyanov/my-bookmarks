namespace MyBookmarks.ViewModels.Category
{
    public class CategoryIndexViewModel
    {
        public List<CategoryListItemViewModel> Categories = new();

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// Total page count; zero when there are no results.
        /// </summary>
        /// <remarks>
        /// PageSize must be positive.
        /// </remarks>
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
