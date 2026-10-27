using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Helpers
{
    /// <summary>
    /// Provides normalization of absolute HTTP and HTTPS URLs.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Attempts to normalize an absolute HTTP or HTTPS URL.
        /// </summary>
        /// <param name="input">The URL text, including its HTTP or HTTPS scheme.</param>
        /// <param name="normalizedUrl">
        /// The normalized absolute URL if successful; otherwise, an empty string.
        /// </param>
        /// <returns>
        /// True if the trimmed input can be parsed as an absolute HTTP or HTTPS URL;
        /// otherwise, false.
        /// </returns>
        /// <remarks>
        /// Leading and trailing whitespace is removed.
        /// The method does not add a missing scheme, enforce a URL length limit
        /// or check whether the website is reachable.
        /// </remarks>
        public static bool TryNormalize(string input, out string normalizedUrl)
        {
            normalizedUrl = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (!Uri.TryCreate(input.Trim(), UriKind.Absolute, out Uri? uri))
            {
                return false;
            }

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            normalizedUrl = uri.AbsoluteUri;

            return true;
        }
    }
}
