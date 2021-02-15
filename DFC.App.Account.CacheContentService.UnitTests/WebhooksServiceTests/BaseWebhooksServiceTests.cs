using System;
using System.Collections.Generic;
using DFC.APP.Account.CacheContentService;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.CacheContentService.UnitTests.WebhooksServiceTests
{
    public abstract class BaseWebhooksServiceTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";
        protected Guid SharedId = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788");

        protected BaseWebhooksServiceTests()
        {
            Logger = A.Fake<ILogger<WebhooksService>>();
            FakeContentCacheService = A.Fake<IContentCacheService>();
            FakeWebhookContentProcessor = A.Fake<IWebhookContentProcessor>();
            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig,  SharedId.ToString()}
            };
            
            Config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
        }

        protected ILogger<WebhooksService> Logger { get; }

        protected IContentCacheService FakeContentCacheService { get; }

        protected IWebhookContentProcessor FakeWebhookContentProcessor { get; }

        protected IConfiguration Config { get; }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(Logger, FakeContentCacheService, FakeWebhookContentProcessor, Config);

            return service;
        }
    }
}