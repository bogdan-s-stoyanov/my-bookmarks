namespace MyBookmarks.Common.EntityValidation
{
    /// <summary>
    /// Defines shared field length limits for bookmarks.
    /// </summary>
    public static class BookmarkValidationConstants
    {
        public const int TitleMaxLength = 100;
        /// <summary>
        /// Maximum stored URL length, including changes made by normalization.
        /// </summary>
        public const int UrlMaxLength = 2048;
        public const int DescriptionMaxLength = 500;
    }
}
