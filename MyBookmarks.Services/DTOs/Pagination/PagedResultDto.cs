using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Pagination
{
    /// <summary>
    /// Contains one page of results and the information needed for pagination.
    /// </summary>
    /// <typeparam name="T">The type of each result item.</typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Gets the items on the current page, which may be an empty list.
        /// </summary>
        public List<T> Items = new();

        /// <summary>
        /// Gets or sets the total number of matching items across all pages, before pagination.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number, starting at 1.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items per page.
        /// </summary>
        public int PageSize { get; set; }
    }
}
