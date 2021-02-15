using System;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DFC.App.Account.CacheContentService.UnitTests.WebhookContentProcessorTests
{
    public class WebhookContentProcessorDeleteContentTests : BaseWebhookContentProcessor
    {
        [Fact]
        public async Task WebhookContentProcessorDeleteContentAsyncReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var service = BuildWebhookContentProcessor();

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(SharedId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(expectedResponse, result);
        }

        [Fact]
        public async Task WebhookContentProcessorDeleteContentAsyncForCreateReturnsNoContent()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
            var service = BuildWebhookContentProcessor();

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(SharedId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(expectedResponse, result);
        }
    }
}
