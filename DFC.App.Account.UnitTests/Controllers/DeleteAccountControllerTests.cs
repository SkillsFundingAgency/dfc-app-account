using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using Moq;
using FakeItEasy;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class DeleteAccountControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IDssWriter _dssService;
        //private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;
        private ISharedContentRedisInterface _sharedContentRedisInterface;

        [SetUp]
        public void Init()
        {
            //_documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            _sharedContentRedisInterface =A. Fake<ISharedContentRedisInterface>();
            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _dssService = Substitute.For<IDssWriter>();
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new DeleteAccountController(_compositeSettings,_dssService, _authService, _config, _sharedContentRedisInterface);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();

            result.Url.Should().Be("~/home");
        }
        [Test]
        public async Task WhenAccountClosedCalled_ReturnHtml()
        {
            var controller = new DeleteAccountController(_compositeSettings, _dssService, _authService, _config, _sharedContentRedisInterface);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.AccountClosed() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalled_AccountDeleted()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new DeleteAccountController(_compositeSettings, _dssService,_authService, _config, _sharedContentRedisInterface);


            var deleteAccountCompositeViewModel = new DeleteAccountCompositeViewModel
            {
                CustomerId = Guid.Parse("c2e27821-cc60-4d3d-b4f0-cbe20867897c")
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var result =  await controller.Body(deleteAccountCompositeViewModel) as RedirectResult;
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/home/signOut?accountClosed=true");

        }

        [Test]
        public async Task WhenPostBodyCalledAndModelIdDoesNotMatchLoggedInId_ThenRedirectToHome()
        {
            var customer = new Customer() { CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c") };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new DeleteAccountController(_compositeSettings, _dssService, _authService, _config, _sharedContentRedisInterface);

            var deleteAccountCompositeViewModel = new DeleteAccountCompositeViewModel
            {
                CustomerId = Guid.Empty
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(deleteAccountCompositeViewModel) as RedirectResult;
            result.Should().NotBeNull();
            result.Url.Should().Be("~/home");

        }

    }
}
