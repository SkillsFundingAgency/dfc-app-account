using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;

namespace DFC.APP.Account.Data.Contracts
{
    public interface ICacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);

        Task<CmsApiSharedContentModel>? GetSharedContentAsync();

        Task ProcessSummaryListAsync(CmsApiSharedContentModel sharedContent, CancellationToken stoppingToken);

        Task GetAndSaveItemAsync(CmsApiSharedContentModel item, CancellationToken stoppingToken);
    }
}