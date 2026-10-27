using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Results
{
    /// <summary>
    /// Identifies the outcome of bookmark editing.
    /// </summary>
    public enum BookmarkEditResult
    {
        /// <summary>
        /// The bookmark was successfully updated.
        /// </summary>
        Success,
        /// <summary>
        /// The bookmark is missing or belongs to another user.
        /// </summary>
        NotFound,
        /// <summary>
        /// The URL is not an absolute HTTP or HTTPS URL, or exceeds the allowed length after normalization.
        /// </summary>
        InvalidUrl,
        /// <summary>
        /// Another bookmark owned by the user has the same trimmed title.
        /// </summary>
        DuplicateTitle,
        /// <summary>
        /// Another bookmark owned by the user has the same normalized URL.
        /// </summary>
        DuplicateUrl,
        /// <summary>
        /// The selected category is missing or belongs to another user.
        /// </summary>
        InvalidCategory
    }
}
