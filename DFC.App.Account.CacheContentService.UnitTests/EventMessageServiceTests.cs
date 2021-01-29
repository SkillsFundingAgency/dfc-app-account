using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DFC.APP.Account.CacheContentService;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using Xunit;

namespace DFC.App.Account.CacheContentService.UnitTests
{
    public class EventMessageServiceTests
    {
        private readonly ILogger<EventMessageService<CmsApiSharedContentModel>> fakeLogger = A.Fake<ILogger<EventMessageService<CmsApiSharedContentModel>>>();
        private readonly IDocumentService<CmsApiSharedContentModel> fakeDcoumentService = A.Fake<IDocumentService<CmsApiSharedContentModel>>();

        [Fact]
        public async Task EventMessageServiceGetAllCachedItemsReturnsSuccess()
        {
            // arrange
            var expectedResult = A.CollectionOfFake<CmsApiSharedContentModel>(2);

            A.CallTo(() => fakeDcoumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResult);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.GetAllCachedItemsAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsSuccess()
        {
            // arrange
            CmsApiSharedContentModel? existingContentPageModel = null;
            var contentPageModel = A.Fake<CmsApiSharedContentModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored,A<string>.Ignored)).Returns(existingContentPageModel);
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(expectedResult);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsBadRequestWhenNullSupplied()
        {
            // arrange
            CmsApiSharedContentModel? contentPageModel = null;
            var expectedResult = HttpStatusCode.BadRequest;

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsAlreadyReportedWhenAlreadyExists()
        {
            // arrange
            var existingContentPageModel = A.Fake<CmsApiSharedContentModel>();
            var contentPageModel = A.Fake<CmsApiSharedContentModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(existingContentPageModel);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsSuccessForSamePartitionKey()
        {
            // arrange
            var existingContentPageModel = A.Fake<CmsApiSharedContentModel>();
            var contentPageModel = A.Fake<CmsApiSharedContentModel>();
            var expectedResult = HttpStatusCode.OK;
            contentPageModel.PartitionKey = "a-partition-key";
            existingContentPageModel.PartitionKey = contentPageModel.PartitionKey;

            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(existingContentPageModel);
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(expectedResult);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDcoumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
        
        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsBadRequestWhenNullSupplied()
        {
            // arrange
            CmsApiSharedContentModel? contentPageModel = null;
            var expectedResult = HttpStatusCode.BadRequest;

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsNotFoundWhenNotExists()
        {
            // arrange
            CmsApiSharedContentModel? existingContentPageModel = null;
            var contentPageModel = A.Fake<CmsApiSharedContentModel>();
            var expectedResult = HttpStatusCode.NotFound;

            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(existingContentPageModel);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDcoumentService.UpsertAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceDeleteAsyncReturnsSuccess()
        {
            // arrange
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeDcoumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceDeleteAsyncReturnsNotFound()
        {
            // arrange
            var expectedResult = HttpStatusCode.NotFound;

            A.CallTo(() => fakeDcoumentService.DeleteAsync(A<Guid>.Ignored)).Returns(false);

            var eventMessageService = new EventMessageService<CmsApiSharedContentModel>(fakeLogger, fakeDcoumentService);

            // act
            var result = await eventMessageService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeDcoumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
