using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Results
{
    public enum BookmarkEditResult
    {
        Success,
        NotFound,
        InvalidUrl,
        DuplicateTitle,
        DuplicateUrl,
        InvalidCategory
    }
}
