using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Bookmark
{
    public class BookmarkDeleteViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public string? Title { get; set; }
    }
}
