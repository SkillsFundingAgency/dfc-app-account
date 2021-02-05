using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.APP.Account.CacheContentService
{
    public class CacheReloadService : ICacheReloadService
    {
        private readonly ILogger<CacheReloadService> logger;
        private readonly IEventMessageService<CmsApiSharedContentModel> eventMessageService;
        private readonly ICmsApiService cmsApiService;
        private readonly IContentTypeMappingService contentTypeMappingService;
        private readonly Guid SharedContent;

        public CacheReloadService(
            ILogger<CacheReloadService> logger,
            IEventMessageService<CmsApiSharedContentModel> eventMessageService,
            ICmsApiService cmsApiService,
            IContentTypeMappingService contentTypeMappingService,
            IConfiguration config)
        {
            this.logger = logger;
            this.eventMessageService = eventMessageService;
            this.cmsApiService = cmsApiService;
            this.contentTypeMappingService = contentTypeMappingService;
            this.SharedContent = config.GetValue<Guid>(Constants.SharedContentGuidConfig);
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload cache started");

                contentTypeMappingService.AddMapping(Constants.ContentTypeSharedContent, typeof(CmsApiSharedContentModel));
                
                var sharedContent = await GetSharedContentAsync().ConfigureAwait(false);

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload cache cancelled");

                    return;
                }

                if (sharedContent != null)
                {
                    await ProcessSummaryListAsync(sharedContent, stoppingToken).ConfigureAwait(false);
                }

                logger.LogInformation("Reload cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in cache reload");
            }
        }

        public async Task<CmsApiSharedContentModel>? GetSharedContentAsync()
        {
            logger.LogInformation("Get shared content");
            var summaryList = await cmsApiService.GetItemAsync<CmsApiSharedContentModel>("sharedContent", SharedContent).ConfigureAwait(false);

            logger.LogInformation("Get shared content completed");

            return summaryList;
        }

        public async Task ProcessSummaryListAsync(CmsApiSharedContentModel sharedContent, CancellationToken stoppingToken)
        {
            logger.LogInformation("Process summary list started");
            
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning("Process summary list cancelled");

                return;
            }

            await GetAndSaveItemAsync(sharedContent, stoppingToken).ConfigureAwait(false);


            logger.LogInformation("Process summary list completed");
        }

        public async Task GetAndSaveItemAsync(CmsApiSharedContentModel item, CancellationToken stoppingToken)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            try
            {
                
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Process item get and save cancelled");

                    return;
                }
                
                logger.LogInformation($"Updating cache with {item.Title} - {item.Url}");
                item.Id = item.ItemId.Value;
                var result = await eventMessageService.UpdateAsync(item).ConfigureAwait(false);

                if (result == HttpStatusCode.NotFound)
                {
                    logger.LogInformation($"Does not exist, creating cache with {item.Title} - {item.Url}");

                    result = await eventMessageService.CreateAsync(item).ConfigureAwait(false);

                    if (result == HttpStatusCode.Created)
                    {
                        logger.LogInformation($"Created cache with {item.Title} - {item.Url}");
                    }
                    else
                    {
                        logger.LogError($"Cache create error status {result} from {item.Title} - {item.Url}");
                    }
                }
                else
                {
                    logger.LogInformation($"Updated cache with {item.Title} - {item.Url}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error in get and save for {item.Title} - {item.Url}");
            }
        }
    }
}
