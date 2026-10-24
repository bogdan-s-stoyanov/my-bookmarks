using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    public class BookmarkEditDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }
    }
}
