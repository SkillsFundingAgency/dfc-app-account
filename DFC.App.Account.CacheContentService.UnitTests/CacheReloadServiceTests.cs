using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DFC.APP.Account.CacheContentService;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Xunit;

namespace DFC.App.Account.CacheContentService.UnitTests
{
    public class CacheReloadServiceTests
    {
        private readonly ILogger<CacheReloadService> fakeLogger = A.Fake<ILogger<CacheReloadService>>();
        private readonly IEventMessageService<CmsApiSharedContentModel> fakeEventMessageService = A.Fake<IEventMessageService<CmsApiSharedContentModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();
        private readonly IContentTypeMappingService fakeContentTypeMappingService = A.Fake<IContentTypeMappingService>();
        private IConfiguration _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            })
            .Build();
        
        [Fact]
        public async Task CacheReloadServiceReloadIsSuccessfulForCreate()
        {
            // arrange
            var cancellationToken = new CancellationToken(false);
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(expectedValidCmsApiSharedContentModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored))
                .Returns(HttpStatusCode.NotFound);
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored))
                .Returns(HttpStatusCode.Created);

            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService,
                fakeContentTypeMappingService, _config);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored))
                .MustHaveHappenedOnceOrMore();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CacheReloadServiceReloadIsSuccessfulForUpdate()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(false);
            var fakeCmsApiSharedContentModel = BuldFakeCmsApiSharedContentModel();
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(expectedValidCmsApiSharedContentModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.OK);
            A.CallTo(() => fakeEventMessageService.GetAllCachedItemsAsync()).Returns(fakeCmsApiSharedContentModel);
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService, fakeContentTypeMappingService, _config);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadIsCancelled()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 1;
            var cancellationToken = new CancellationToken(true);
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();
            var fakeCmsApiSharedContentModel = BuldFakeCmsApiSharedContentModel();


            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(expectedValidCmsApiSharedContentModel);

            A.CallTo(() => fakeEventMessageService.GetAllCachedItemsAsync()).Returns(fakeCmsApiSharedContentModel);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService, fakeContentTypeMappingService, _config);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentTypeMappingService.AddMapping(A<string>.Ignored, A<Type>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsSuccessfulForCreate()
        {
            // arrange
            var cancellationToken = new CancellationToken(false);
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();

            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService, fakeContentTypeMappingService, _config);

            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidCmsApiSharedContentModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsSuccessfulForUpdate()
        {
            // arrange
            var cancellationToken = new CancellationToken(false);
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();

            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService, fakeContentTypeMappingService, _config);

            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidCmsApiSharedContentModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsCancelled()
        {
            // arrange
            var cancellationToken = new CancellationToken(true);
            var expectedValidCmsApiSharedContentModel = BuildValidCmsApiSharedContentModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(expectedValidCmsApiSharedContentModel);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeEventMessageService, fakeCmsApiService, fakeContentTypeMappingService, _config);
            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidCmsApiSharedContentModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiSharedContentModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<CmsApiSharedContentModel>.Ignored)).MustNotHaveHappened();
        }


        private static CmsApiSharedContentModel BuildValidCmsApiSharedContentModel()
        {
            var model = new CmsApiSharedContentModel()
            {
                Title = "an-article",
                Url = new Uri("/aaa/bbb", UriKind.Relative),
                Published = DateTime.Now,
                CreatedDate = DateTime.Now,
                ItemId = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788"),
                Id = new Guid("2c9da1b3-3529-4834-afc9-9cd741e59788"),
            };

            return model;
        }
        
        private List<CmsApiSharedContentModel> BuldFakeCmsApiSharedContentModel()
        {
            var model = A.Fake<CmsApiSharedContentModel>();

            var id = Guid.NewGuid();

            model.Id = id;
            model.Url = new Uri($"http://somewhere.com/item/{id}", UriKind.Absolute);


            return new List<CmsApiSharedContentModel>()
            {
                {model}
            };
        }
    }
}
