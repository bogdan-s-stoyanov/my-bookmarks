using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.ViewModels.Bookmark
{
    public class BookmarkListItemViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public bool IsFavorite { get; set; }
    }
}
