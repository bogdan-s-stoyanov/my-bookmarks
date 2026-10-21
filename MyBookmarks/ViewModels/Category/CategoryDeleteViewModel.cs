using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    public class CategoryDeleteViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
