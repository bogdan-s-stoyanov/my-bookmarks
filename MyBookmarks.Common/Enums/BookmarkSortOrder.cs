using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Common.Enums
{
    /// <summary>
    /// Defines the available orderings for bookmark lists.
    /// </summary>
    public enum BookmarkSortOrder
    {
        /// <summary>
        /// Orders by title in ascending order, then by ID in ascending order.
        /// </summary>
        TitleAscending,
        /// <summary>
        /// Orders by title in descending order, then by ID in descending order.
        /// </summary>
        TitleDescending,
        /// <summary>
        /// Orders by creation time in ascending order, then by ID in ascending order.
        /// </summary>
        Oldest,
        /// <summary>
        /// Orders by creation time in descending order, then by ID in descending order.
        /// </summary>
        Newest
    }
}
