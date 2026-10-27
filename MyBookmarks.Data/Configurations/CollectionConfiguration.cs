using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data.Configurations
{
    /// <summary>
    /// Configures collection ownership deletion restrictions and unique names per owner.
    /// </summary>
    internal class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        /// <summary>
        /// Configures collection ownership deletion restrictions and unique names per owner.
        /// </summary>
        /// <param name="builder">The entity builder for collection entities.</param>
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
        }
    }
}