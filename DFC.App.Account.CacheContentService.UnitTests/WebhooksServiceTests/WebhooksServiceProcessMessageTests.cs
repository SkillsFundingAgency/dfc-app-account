using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DFC.APP.Account.Data;
using DFC.Content.Pkg.Netcore.Data.Enums;
using FakeItEasy;
using NUnit.Framework;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DFC.App.Account.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service ProcessMessageAsync Unit Tests")]
    public class WebhooksServiceProcessMessageTests : BaseWebhooksServiceTests
    {

        
        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncNoneOptionReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();


            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), SharedId, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceProcessMessageAsyncContentThrowsErrorForInvalidUrl()
        {
            // Arrange
            const ContentCacheStatus isContentItem = ContentCacheStatus.ContentItem;
            var url = "/somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);

            // Act
            Assert.ThrowsAsync<InvalidDataException>(() =>  service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), SharedId, url));
            // Assert
            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateReturnsSuccess()
        {
            // Arrange
            const ContentCacheStatus isContentItem = ContentCacheStatus.Content;
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), SharedId, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentUpdateReturnsSuccess()
        {
            // Arrange
            const ContentCacheStatus isContentItem = ContentCacheStatus.Content;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();


            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), SharedId, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]

        public async Task WebhooksServiceProcessMessageAsyncContentDeleteReturnsSuccess()
        {
            // Arrange
            const ContentCacheStatus isContentItem = ContentCacheStatus.Content;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), SharedId, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhookContentProcessor.DeleteContentAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeWebhookContentProcessor.ProcessContentAsync(A<Uri>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(expectedResponse, result);
        }

    }
}
