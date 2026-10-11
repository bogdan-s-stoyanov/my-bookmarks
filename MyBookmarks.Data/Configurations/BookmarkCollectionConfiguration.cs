using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    internal class BookmarkCollectionConfiguration : IEntityTypeConfiguration<BookmarkCollection>
    {
        public void Configure(EntityTypeBuilder<BookmarkCollection> builder)
        {
            builder.HasKey(bc => new
            {
                bc.BookmarkId,
                bc.CollectionId
            });
        }
    }
}
