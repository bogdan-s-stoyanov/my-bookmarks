using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    internal class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.HasOne(b => b.User)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(b => new { b.UserId, b.Title })
                .IsUnique();
        }
    }
}
