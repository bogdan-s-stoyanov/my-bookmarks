using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs
{
    public class CategoryPageDto 
    {
        public List<CategoryListItemDto> Categories { get; set; } = new();

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
