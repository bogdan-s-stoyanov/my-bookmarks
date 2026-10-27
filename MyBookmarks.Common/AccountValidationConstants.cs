namespace MyBookmarks.Common
{
    /// <summary>
    /// Defines password length limits used by account validation.
    /// </summary>
    public static class AccountValidationConstants
    {
        /// <summary>
        /// The minimum permitted password length in characters.
        /// </summary>
        public const int PasswordMinLength = 8;
        /// <summary>
        /// The maximum password length accepted by the registration form.
        /// </summary>
        public const int PasswordMaxLength = 100;
    }
}
