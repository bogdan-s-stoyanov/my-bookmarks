using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Results
{
    public enum BookmarkCreateResult
    {
        Success,
        InvalidUrl,
        DuplicateTitle,
        DuplicateUrl,
        InvalidCategory
    }
}
