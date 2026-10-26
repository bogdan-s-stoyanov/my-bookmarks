using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Contracts
{
    public interface ICollectionService
    {
        Task<PagedResultDto<CollectionListItemDto>>
            GetUserCollectionsAsync(string userId, int pageNumber);

        Task<bool> TryCreateAsync(string userId, CollectionCreateDto dto);

        Task<CollectionEditDto?> GetForEditAsync(string userId, int id);

        Task<CollectionEditResult> EditAsync(string userId, CollectionEditDto dto);

        Task<CollectionDeleteDto?> GetForDeleteAsync(string userId, int id);

        Task<bool> TryDeleteAsync(string userId, int id);

        Task<CollectionDetailsDto?> GetDetailsAsync(string userId, int id, int pageNumber);

        Task<bool> TryAddBookmarkAsync(string userId, int collectionId, int bookmarkId);

        Task<CollectionListItemDto?> GetByIdAsync(string userId, int id);

        Task<List<int>> GetIncludedBookmarkIdsAsync
            (string userId, int collectionId, List<int> bookmarkIds);

        Task<bool> TryRemoveBookmarkAsync(string userId, int collectionId, int bookmarkId);
    }
}
