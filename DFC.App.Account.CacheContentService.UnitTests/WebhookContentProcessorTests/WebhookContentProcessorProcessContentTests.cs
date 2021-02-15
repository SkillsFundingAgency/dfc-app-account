using DFC.APP.Account.Data.Models;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using NSubstitute.ReturnsExtensions;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DFC.App.Account.CacheContentService.UnitTests.WebhookContentProcessorTests
{
    public class WebhookContentProcessorProcessContentTests : BaseWebhookContentProcessor
    {
        [Fact]
        public async Task WebhookContentProcessorProcessContentAsyncForCreateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidApiContentModel = BuildValidCmsApiSharedContentModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhookContentProcessor();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessContentAsync(url, SharedId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]
        public async Task WebhookContentProcessorProcessContentAsyncForUpdateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiContentModel = BuildValidCmsApiSharedContentModel();

            var url = new Uri("https://somewhere.com");
            var service = BuildWebhookContentProcessor();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessContentAsync(url, SharedId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]
        public async Task WebhookContentProcessorProcessContentAsyncForUpdateReturnsNoContent()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
            var expectedValidApiContentModel = BuildValidCmsApiSharedContentModel();
            ContentPageModel? expectedValidContentPageModel = default;
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhookContentProcessor();
            CmsApiSharedContentModel content;
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).Returns(Task.FromResult<CmsApiSharedContentModel>(null));

            // Act
            var result = await service.ProcessContentAsync(url, SharedId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(expectedResponse, result);
        }
    }
}
