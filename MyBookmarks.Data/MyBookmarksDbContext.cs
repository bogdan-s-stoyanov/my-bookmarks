using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Configurations;
using MyBookmarks.Data.Models;

namespace MyBookmarks.Data
{
    /// <summary>
    /// Provides database access to Identity accounts, bookmarks, categories, collections and their links.
    /// </summary>
    public class MyBookmarksDbContext : IdentityDbContext
    {
        public MyBookmarksDbContext(DbContextOptions<MyBookmarksDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BookmarkCollectionConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new BookmarkConfiguration());
            builder.ApplyConfiguration(new CollectionConfiguration());
        }

        public DbSet<Bookmark> Bookmarks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<BookmarkCollection> BookmarkCollections { get; set; }
    }
}
