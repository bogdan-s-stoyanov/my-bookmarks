using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Results
{
    /// <summary>
    /// Identifies the outcome of category editing.
    /// </summary>
    public enum CategoryEditResult
    {
        /// <summary>
        /// The category was successfully updated.
        /// </summary>
        Success,
        /// <summary>
        /// The category is missing or belongs to another user.
        /// </summary>
        NotFound,
        /// <summary>
        /// Another category owned by the user has the same trimmed name.
        /// </summary>
        DuplicateName
    }
}
