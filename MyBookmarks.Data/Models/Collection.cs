using Microsoft.AspNetCore.Identity;
using MyBookmarks.Data.Common.EntityValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookmarks.Data.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CollectionValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(CollectionValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public ICollection<BookmarkCollection> BookmarkCollections { get; set; }
           = new HashSet<BookmarkCollection>();
    }
}
