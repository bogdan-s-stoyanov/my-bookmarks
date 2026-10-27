namespace MyBookmarks.Common.EntityValidation
{
    /// <summary>
    /// Defines shared field length limits for bookmarks.
    /// </summary>
    public static class BookmarkValidationConstants
    {
        /// <summary>
        /// The maximum bookmark title length in characters.
        /// </summary>
        public const int TitleMaxLength = 100;
        /// <summary>
        /// The maximum saved URL length in characters, including any normalization changes.
        /// </summary>
        public const int UrlMaxLength = 2048;
        /// <summary>
        /// The maximum bookmark description length in characters.
        /// </summary>
        public const int DescriptionMaxLength = 500;
    }
}
