using MyBookmarks.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Category
{
    public class CategoryListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
