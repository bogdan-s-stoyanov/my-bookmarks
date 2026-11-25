namespace MyBookmarks.ViewModels
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Identifies the failed request for correlation with application logs.
        /// </summary>
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
