using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Configurations
{
    /// <summary>
    /// Configures the composite primary key for bookmark-to-collection links.
    /// </summary>
    internal class BookmarkCollectionConfiguration : IEntityTypeConfiguration<BookmarkCollection>
    {
        /// <summary>
        /// Configures the composite primary key for bookmark-to-collection links.
        /// </summary>
        /// <param name="builder">The entity builder for bookmark-to-collection links.</param>
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
