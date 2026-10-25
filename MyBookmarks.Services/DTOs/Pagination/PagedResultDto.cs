using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Pagination
{
    public class PagedResultDto<T>
    {
        public List<T> Items = new();

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
