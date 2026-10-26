using Microsoft.AspNetCore.Mvc;
using MyBookmarks.ViewModels.Category;

namespace MyBookmarks.ViewModels.Bookmark
{
    public class BookmarkIndexViewModel
    {
        public List<BookmarkListItemViewModel> Bookmarks { get; set; } = new();

        public int PageNumber { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
            => (int)Math.Ceiling((double)TotalCount / PageSize);

        public string? SearchTerm { get; set; }

        public int? CategoryId { get; set; }

        public List<CategoryListItemViewModel> Categories { get; set; } = new();
    }
}
