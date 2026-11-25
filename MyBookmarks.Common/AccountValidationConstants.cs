namespace MyBookmarks.Common
{
    /// <summary>
    /// Defines password length limits used by account validation.
    /// </summary>
    public static class AccountValidationConstants
    {
        public const int PasswordMinLength = 8;
        /// <summary>
        /// Maximum password length accepted by the registration form.
        /// </summary>
        public const int PasswordMaxLength = 100;
    }
}
