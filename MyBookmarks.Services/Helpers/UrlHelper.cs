using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Helpers
{
    public static class UrlHelper
    {
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
