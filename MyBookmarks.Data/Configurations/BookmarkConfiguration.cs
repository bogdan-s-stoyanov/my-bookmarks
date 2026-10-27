using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    /// <summary>
    /// Configures bookmark ownership deletion restrictions and unique titles per owner.
    /// </summary>
    internal class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        /// <summary>
        /// Configures bookmark ownership deletion restrictions and unique titles per owner.
        /// </summary>
        /// <param name="builder">The entity builder for bookmark entities.</param>
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.HasOne(b => b.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => new { b.UserId, b.Title })
                .IsUnique();
        }
    }
}
