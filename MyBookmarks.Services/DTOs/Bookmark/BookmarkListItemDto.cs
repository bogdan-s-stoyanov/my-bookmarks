using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    public class BookmarkListItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public bool IsFavourite { get; set; }
    }
}
