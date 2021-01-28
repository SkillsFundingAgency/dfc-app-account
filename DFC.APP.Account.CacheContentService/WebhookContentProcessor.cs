using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.APP.Account.CacheContentService
{
    public class WebhookContentProcessor : IWebhookContentProcessor
    {
        private readonly ILogger<WebhookContentProcessor> logger;
        private readonly IEventMessageService<CmsApiSharedContentModel> eventMessageService;
        private readonly ICmsApiService cmsApiService;

        public WebhookContentProcessor(
            ILogger<WebhookContentProcessor> logger,
            IEventMessageService<CmsApiSharedContentModel> eventMessageService,
            ICmsApiService cmsApiService)
        {
            this.logger = logger;
            this.eventMessageService = eventMessageService;
            this.cmsApiService = cmsApiService;
        }

        public async Task<HttpStatusCode> ProcessContentAsync(Uri url, Guid contentId)
        {
            var apiDataModel = await cmsApiService.GetItemAsync<CmsApiSharedContentModel>(url).ConfigureAwait(false);

            apiDataModel.Id = apiDataModel.ItemId.Value;

            if (apiDataModel == null)
            {
                return HttpStatusCode.NoContent;
            }

            var contentResult = await eventMessageService.UpdateAsync(apiDataModel).ConfigureAwait(false);

            if (contentResult == HttpStatusCode.NotFound)
            {
                contentResult = await eventMessageService.CreateAsync(apiDataModel).ConfigureAwait(false);
            }


            return contentResult;
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            var result = await eventMessageService.DeleteAsync(contentId).ConfigureAwait(false);
            
            return result;
        }

    }
}
