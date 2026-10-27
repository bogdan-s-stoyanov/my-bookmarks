using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services
{
    /// <summary>
    /// Implements collection operations and ownership checks for the current user.
    /// </summary>
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository collectionRepository;
        private readonly IBookmarkRepository bookmarkRepository;

        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="collectionRepository">The repository used to manage collections and bookmark links.</param>
        /// <param name="bookmarkRepository">The repository used to retrieve bookmarks and check their ownership.</param>
        public CollectionService(ICollectionRepository collectionRepository, IBookmarkRepository bookmarkRepository)
        {
            this.collectionRepository = collectionRepository;
            this.bookmarkRepository = bookmarkRepository;
        }

        /// <inheritdoc />
        public async Task<PagedResultDto<CollectionListItemDto>> 
            GetUserCollectionsAsync(string userId, int pageNumber)
        {
            int pageSize = PageConstants.CollectionsPageSize;

            int totalCount = await collectionRepository
                .CountCollectionsForUserAsync(userId);

            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            int skip = (pageNumber - 1) * pageSize;

            List<Collection> collections = await collectionRepository
                .GetCollectionsByUserIdAsync(userId, pageSize, skip);

            return new PagedResultDto<CollectionListItemDto>
            {
                Items = collections
                    .Select(c => new CollectionListItemDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToList(),

                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <inheritdoc />
        public async Task<bool> TryCreateAsync(string userId, CollectionCreateDto dto)
        {
            string name = dto.Name.Trim();

            bool alreadyExists = await this.collectionRepository
                .ExistsByNameAsync(userId, name);

            if (alreadyExists)
            {
                return false;
            }

            Collection collection = new Collection
            {
                Name = name,
                Description = dto.Description?.Trim(),
                UserId = userId
            };

            await collectionRepository.AddAsync(collection);

            return true;
        }

        /// <inheritdoc />
        public async Task<CollectionEditDto?> GetForEditAsync(string userId, int id)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return null;
            }

            return new CollectionEditDto
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description
            };
        }

        /// <inheritdoc />
        public async Task<CollectionEditResult> EditAsync(string userId, CollectionEditDto dto)
        {
            Collection? collection = await this.collectionRepository
                .GetByIdAsync(userId, dto.Id);

            if (collection == null)
            {
                return CollectionEditResult.NotFound;
            }

            string name = dto.Name.Trim();

            bool nameExists = await this.collectionRepository
                .ExistsByNameExceptIdAsync(userId, name, dto.Id);

            if (nameExists)
            {
                return CollectionEditResult.DuplicateName;
            }

            collection.Name = name;
            collection.Description = dto.Description?.Trim();

            await collectionRepository.UpdateAsync(collection);

            return CollectionEditResult.Success;
        }

        /// <inheritdoc />
        public async Task<CollectionDeleteDto?> GetForDeleteAsync(string userId, int id)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return null;
            }

            return new CollectionDeleteDto
            {
                Id = collection.Id,
                Name = collection.Name
            };
        }

        /// <inheritdoc />
        public async Task<bool> TryDeleteAsync(string userId, int id)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return false;
            }

            await collectionRepository.DeleteAsync(collection);

            return true;
        }

        /// <inheritdoc />
        public async Task<CollectionDetailsDto?> GetDetailsAsync(string userId, int id, int pageNumber)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return null;
            }

            int pageSize = PageConstants.BookmarksPageSize;

            int totalCount = await bookmarkRepository
                .CountByCollectionAsync(userId, id);

            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            int skip = (pageNumber - 1) * pageSize;

            List<Bookmark> bookmarks = await bookmarkRepository
                .GetByCollectionAsync(userId, id, pageSize, skip);

            return new CollectionDetailsDto
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description,

                BookmarksPage = new PagedResultDto<BookmarkListItemDto>
                {
                    Items = bookmarks
                        .Select(b => new BookmarkListItemDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Url = b.Url,
                            Description = b.Description,
                            CategoryName = b.Category?.Name,
                            IsFavorite = b.IsFavorite
                        })
                        .ToList(),

                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            };
        }

        /// <inheritdoc />
        public async Task<bool> TryAddBookmarkAsync(string userId, int collectionId, int bookmarkId)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, collectionId);

            if (collection == null)
            {
                return false;
            }

            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, bookmarkId);

            if (bookmark == null)
            {
                return false;
            }

            bool alreadyIncluded = await collectionRepository
                .ContainsBookmarkAsync(collectionId, bookmarkId);

            if (alreadyIncluded)
            {
                return true;
            }

            BookmarkCollection membership = new BookmarkCollection
            {
                CollectionId = collectionId,
                BookmarkId = bookmarkId
            };

            await collectionRepository.AddBookmarkAsync(membership);

            return true;
        }

        /// <inheritdoc />
        public async Task<CollectionListItemDto?> GetByIdAsync(string userId, int id)
        {
            Collection? collection = await this.collectionRepository
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return null;
            }

            return new CollectionListItemDto
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description
            };
        }

        /// <inheritdoc />
        public async Task<List<int>> GetIncludedBookmarkIdsAsync(string userId, int collectionId, List<int> bookmarkIds)
        {
            return await collectionRepository.
                GetIncludedBookmarkIdsAsync(userId, collectionId, bookmarkIds);
        }

        /// <inheritdoc />
        public async Task<bool> TryRemoveBookmarkAsync(string userId, int collectionId, int bookmarkId)
        {
            Collection? collection = await collectionRepository
                .GetByIdAsync(userId, collectionId);

            if (collection == null)
            {
                return false;
            }

            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, bookmarkId);

            if (bookmark == null)
            {
                return false;
            }

            BookmarkCollection? membership = await collectionRepository
                .GetBookmarkCollectionAsync(collectionId, bookmarkId);

            if (membership == null)
            {
                return true;
            }

            await collectionRepository
                .RemoveBookmarkFromCollection(membership);

            return true;
        }
    }
}
