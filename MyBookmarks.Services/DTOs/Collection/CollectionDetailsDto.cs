using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    public class CollectionDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public PagedResultDto<BookmarkListItemDto> BookmarksPage { get; set; }
            = new();
    }
}
