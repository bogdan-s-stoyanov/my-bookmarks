namespace MyBookmarks.Common
{
    /// <summary>
    /// Defines the number of items requested per page for the application's lists.
    /// </summary>
    public static class PageConstants
    {
        /// <summary>
        /// The maximum number of categories per page.
        /// </summary>
        public const int CategoriesPageSize = 20;
        /// <summary>
        /// The maximum number of bookmarks per page.
        /// </summary>
        public const int BookmarksPageSize = 12;

        /// <summary>
        /// The maximum number of collections per page.
        /// </summary>
        public const int CollectionsPageSize = 12;
    }
}
