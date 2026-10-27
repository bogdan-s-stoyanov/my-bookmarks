using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    /// <summary>
    /// Configures category ownership, unique names per owner and uncategorizing bookmarks on category deletion.
    /// </summary>
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Configures category ownership, unique names per owner and uncategorizing bookmarks on category deletion.
        /// </summary>
        /// <param name="builder">The entity builder for category entities.</param>
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.Bookmarks)
                .WithOne(b => b.Category)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
        }
    }
}
