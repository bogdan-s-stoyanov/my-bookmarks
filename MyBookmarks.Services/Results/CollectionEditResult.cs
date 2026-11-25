using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Results
{
    /// <summary>
    /// Identifies the outcome of collection editing.
    /// </summary>
    public enum CollectionEditResult
    {
        Success,
        /// <summary>
        /// The collection is missing or belongs to another user.
        /// </summary>
        NotFound,
        /// <summary>
        /// Another collection owned by the user has the same trimmed name.
        /// </summary>
        DuplicateName
    }
}
