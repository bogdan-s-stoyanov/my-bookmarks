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
        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="options">The options configuring the database connection and provider.</param>
        public MyBookmarksDbContext(DbContextOptions<MyBookmarksDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Applies the Identity schema and the application's entity configurations.
        /// </summary>
        /// <param name="builder">The model builder used to configure the database mappings.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BookmarkCollectionConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new BookmarkConfiguration());
            builder.ApplyConfiguration(new CollectionConfiguration());
        }

        /// <summary>
        /// Gets or sets the saved bookmarks.
        /// </summary>
        public DbSet<Bookmark> Bookmarks { get; set; }

        /// <summary>
        /// Gets or sets the user-owned bookmark categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the user-owned bookmark collections.
        /// </summary>
        public DbSet<Collection> Collections { get; set; }

        /// <summary>
        /// Gets or sets the links between bookmarks and collections.
        /// </summary>
        public DbSet<BookmarkCollection> BookmarkCollections { get; set; }
    }
}
