namespace MyBookmarks.Common.EntityValidation
{
    /// <summary>
    /// Defines shared field length limits for collections.
    /// </summary>
    public static class CollectionValidationConstants
    {
        /// <summary>
        /// The maximum collection name length in characters.
        /// </summary>
        public const int NameMaxLength = 100;
        /// <summary>
        /// The maximum collection description length in characters.
        /// </summary>
        public const int DescriptionMaxLength = 500;
    }
}
