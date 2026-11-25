using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Common.Enums
{
    /// <summary>
    /// Available orderings for bookmark lists.
    /// </summary>
    /// <remarks>
    /// Ties are resolved by ID in the same direction as the selected ordering.
    /// </remarks>
    public enum BookmarkSortOrder
    {
        TitleAscending,
        TitleDescending,
        Oldest,
        Newest
    }
}
