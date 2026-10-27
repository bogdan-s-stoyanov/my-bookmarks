namespace MyBookmarks.ViewModels
{
    /// <summary>
    /// Contains request information displayed by the error page.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the identifier used to correlate the failed request with diagnostics.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets whether a non-empty request identifier is available for display.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
