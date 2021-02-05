using System;
using DFC.APP.Account.CacheContentService;
using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.CacheContentService.UnitTests.WebhookContentProcessorTests
{
    public class BaseWebhookContentProcessor
    {
        protected BaseWebhookContentProcessor()
        {
            Logger = A.Fake<ILogger<WebhookContentProcessor>>();
            FakeEventMessageService = A.Fake<IEventMessageService<CmsApiSharedContentModel>>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
        }

        protected Guid SharedId = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788");
        protected ILogger<WebhookContentProcessor> Logger { get; }


        protected IEventMessageService<CmsApiSharedContentModel> FakeEventMessageService { get; }

        protected ICmsApiService FakeCmsApiService { get; }
        
        protected static CmsApiSharedContentModel BuildValidCmsApiSharedContentModel()
        {
            var model = new CmsApiSharedContentModel()
            {
                ItemId = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788"),
                Id = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788")
            };

            return model;
        }


        protected WebhookContentProcessor BuildWebhookContentProcessor()
        {
            var service = new WebhookContentProcessor(Logger, FakeEventMessageService, FakeCmsApiService);

            return service;
        }
    }
}
