using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface IBookmarkRepository
    {
        Task<List<Bookmark>> GetBookmarksByUserIdAsync(string userId, int take, int skip);

        Task<int> CountBookmarksForUserAsync(string userId);

        Task AddAsync(Bookmark bookmark);
    }
}
