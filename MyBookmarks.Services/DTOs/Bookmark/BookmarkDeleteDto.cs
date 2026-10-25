using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Bookmark
{
    public class BookmarkDeleteDto 
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
    }
}
