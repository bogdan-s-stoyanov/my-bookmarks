namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionIndexViewModel
    {
        public List<CollectionListItemViewModel> Collections { get; set; }
            = new();

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
