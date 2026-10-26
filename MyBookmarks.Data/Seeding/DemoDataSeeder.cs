using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Seeding
{
    public static class DemoDataSeeder
    {
        private const string DemoEmail = "demo@mybookmarks.test";
        private const string DemoPassword = "Demo123!";
        private const string SeedClaimType = "MyBookmarks.DemoDataSeeded";

        public static async Task SeedAsync(MyBookmarksDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            IdentityUser user = await EnsureDemoUserAsync(userManager);

            bool alreadySeeded = await dbContext.UserClaims
                .AnyAsync(c =>
                    c.UserId == user.Id &&
                    c.ClaimType == SeedClaimType &&
                    c.ClaimValue == "true");

            if (alreadySeeded)
            {
                return;
            }

            List<Category> categories = CreateDemoCategories(user.Id);
            List<Bookmark> bookmarks = CreateDemoBookmarks(user.Id, categories);
            List<Collection> collections = CreateDemoCollections(user.Id, bookmarks);

            dbContext.Categories.AddRange(categories);
            dbContext.Bookmarks.AddRange(bookmarks);
            dbContext.Collections.AddRange(collections);

            dbContext.UserClaims.Add(new IdentityUserClaim<string>
            {
                UserId = user.Id,
                ClaimType = SeedClaimType,
                ClaimValue = "true"
            });

            await dbContext.SaveChangesAsync();
        }

        public static async Task<IdentityUser> EnsureDemoUserAsync(
            UserManager<IdentityUser> userManager)
        {
            IdentityUser? user = await userManager
                .FindByEmailAsync(DemoEmail);

            if (user != null)
            {
                return user;
            }

            user = new IdentityUser
            {
                UserName = DemoEmail,
                Email = DemoEmail,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager
                .CreateAsync(user, DemoPassword);

            if (!result.Succeeded)
            {
                string errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Could not create the demo user: {errors}");
            }

            return user;
        }

        private static List<Category> CreateDemoCategories(string userId)
        {
            return new List<Category>
            {
                new Category
                {
                    Name = "Development",
                    UserId = userId
                },
                new Category
                {
                    Name = "Learning",
                    UserId = userId
                },
                new Category
                {
                    Name = "Tools",
                    UserId = userId
                }
            };
        }

        private static List<Bookmark> CreateDemoBookmarks(string userId, 
            List<Category> categories)
        {
            Category development = categories.Single(c => c.Name == "Development");
            Category learning = categories.Single(c => c.Name == "Learning");
            Category tools = categories.Single(c => c.Name == "Tools");

            List<Bookmark> bookmarks = new()
            {
                new Bookmark
                {
                    Title = "ASP.NET Core documentation",
                    Url = "https://learn.microsoft.com/aspnet/core/",
                    Description = "Documentation for building web applications with ASP.NET Core.",
                    Category = development,
                    IsFavorite = true
                },
                new Bookmark
                {
                    Title = "Entity Framework Core",
                    Url = "https://learn.microsoft.com/ef/core/",
                    Description = "Guides for working with databases using Entity Framework Core.",
                    Category = development,
                    IsFavorite = true
                },
                new Bookmark
                {
                    Title = "C# documentation",
                    Url = "https://learn.microsoft.com/dotnet/csharp/",
                    Description = "C# language reference and programming guides.",
                    Category = development
                },
                new Bookmark
                {
                    Title = "Bootstrap",
                    Url = "https://getbootstrap.com/",
                    Description = "Components and utilities for responsive interfaces.",
                    Category = development
                },
                new Bookmark
                {
                    Title = "MDN Web Docs",
                    Url = "https://developer.mozilla.org/",
                    Description = "Documentation for HTML, CSS and JavaScript.",
                    Category = development,
                    IsFavorite = true
                },
                new Bookmark
                {
                    Title = "Microsoft Learn",
                    Url = "https://learn.microsoft.com/",
                    Description = "Learning paths and tutorials for Microsoft technologies.",
                    Category = learning
                },
                new Bookmark
                {
                    Title = "freeCodeCamp",
                    Url = "https://www.freecodecamp.org/",
                    Description = "Programming exercises and learning resources.",
                    Category = learning,
                    IsFavorite = true
                },
                new Bookmark
                {
                    Title = "Khan Academy",
                    Url = "https://www.khanacademy.org/",
                    Description = "Lessons and exercises across different subjects.",
                    Category = learning
                },
                new Bookmark
                {
                    Title = "Git documentation",
                    Url = "https://git-scm.com/doc",
                    Description = "Learn about version control and Git commands.",
                    Category = learning
                },
                new Bookmark
                {
                    Title = "GitHub Skills",
                    Url = "https://skills.github.com/",
                    Description = "Interactive courses for learning GitHub.",
                    Category = learning
                },
                new Bookmark
                {
                    Title = "GitHub",
                    Url = "https://github.com/",
                    Description = "Host repositories and collaborate on projects.",
                    Category = tools,
                    IsFavorite = true
                },
                new Bookmark
                {
                    Title = "Visual Studio Code",
                    Url = "https://code.visualstudio.com/",
                    Description = "A code editor with extensions for different languages.",
                    Category = tools
                },
                new Bookmark
                {
                    Title = "JSONPlaceholder",
                    Url = "https://jsonplaceholder.typicode.com/",
                    Description = "A sample API for experimenting with HTTP requests.",
                    Category = tools
                },
                new Bookmark
                {
                    Title = "Wikipedia",
                    Url = "https://www.wikipedia.org/",
                    Description = "An online encyclopedia."
                },
                new Bookmark
                {
                    Title = "Internet Archive",
                    Url = "https://archive.org/"
                }
            };

            DateTime now = DateTime.UtcNow;

            for (int i = 0; i < bookmarks.Count; i++)
            {
                bookmarks[i].UserId = userId;
                bookmarks[i].DateAdded = now.AddDays(-i);
            }

            return bookmarks;
        }

        private static List<Collection> CreateDemoCollections(
            string userId, List<Bookmark> bookmarks)
        {
            Bookmark aspNet = bookmarks.Single(
                b => b.Title == "ASP.NET Core documentation");

            Bookmark entityFramework = bookmarks.Single(
                b => b.Title == "Entity Framework Core");

            Bookmark csharp = bookmarks.Single(
                b => b.Title == "C# documentation");

            Bookmark bootstrap = bookmarks.Single(
                b => b.Title == "Bootstrap");

            Bookmark microsoftLearn = bookmarks.Single(
                b => b.Title == "Microsoft Learn");

            Bookmark gitDocumentation = bookmarks.Single(
                b => b.Title == "Git documentation");

            return new List<Collection>
            {
                new Collection
                {
                    Name = "Web development",
                    Description = "Resources for building an ASP.NET Core application.",
                    UserId = userId,
                    BookmarkCollections = new List<BookmarkCollection>
                    {
                        new BookmarkCollection { Bookmark = aspNet },
                        new BookmarkCollection { Bookmark = entityFramework },
                        new BookmarkCollection { Bookmark = bootstrap }
                    }
                },
                new Collection
                {
                    Name = "Learning essentials",
                    Description = "Resources to revisit while learning programming.",
                    UserId = userId,
                    BookmarkCollections = new List<BookmarkCollection>
                    {
                        new BookmarkCollection { Bookmark = aspNet },
                        new BookmarkCollection { Bookmark = csharp },
                        new BookmarkCollection { Bookmark = microsoftLearn },
                        new BookmarkCollection { Bookmark = gitDocumentation }
                    }
                }
            };
        }
    }
}
