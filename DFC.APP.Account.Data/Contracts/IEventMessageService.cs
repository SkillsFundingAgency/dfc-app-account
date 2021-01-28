using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;

namespace DFC.APP.Account.Data.Contracts
{
    public interface IEventMessageService<TModel>
        where TModel : CmsApiSharedContentModel
    {
        Task<IList<TModel>?> GetAllCachedItemsAsync();

        Task<HttpStatusCode> CreateAsync(TModel? upsertDocumentModel);

        Task<HttpStatusCode> UpdateAsync(TModel? upsertDocumentModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}
