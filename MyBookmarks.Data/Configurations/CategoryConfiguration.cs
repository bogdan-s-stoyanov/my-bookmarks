using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
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
