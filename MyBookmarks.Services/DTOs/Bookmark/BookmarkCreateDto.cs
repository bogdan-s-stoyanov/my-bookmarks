using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    public class BookmarkCreateDto
    {
        public string Title { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string? Description { get; set; }

        public int? CategoryId { get; set; }
    }
}
