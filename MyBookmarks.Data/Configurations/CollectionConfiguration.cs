using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data.Configurations
{
    internal class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}