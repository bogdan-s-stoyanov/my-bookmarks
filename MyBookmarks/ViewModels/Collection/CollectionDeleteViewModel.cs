using System.ComponentModel.DataAnnotations;

namespace MyBookmarks.ViewModels.Collection
{
    public class CollectionDeleteViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
