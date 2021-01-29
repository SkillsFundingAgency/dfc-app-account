using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.Auth.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class CloseYourAccountControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IOpenIDConnectClient _openIdConnectClient;
        private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;

        [SetUp]
        public void Init()
        {
            _documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _openIdConnectClient = Substitute.For<IOpenIDConnectClient>();
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var customer = new Customer
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                Contact = new Contact
                {
                    EmailAddress = "user"
                }
            };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).ReturnsForAnyArgs(customer);
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient, _documentService, _config);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            
            result.ViewName.Should().BeNull();
        }


        [Test]
        public async Task WhenBodyCalledWithInvalidModelState_ReturnToViewWithError()
        {
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient, _documentService, _config);
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ModelState.AddModelError("password","Invalid password");
            var result = await controller.Body(closeYourAccountCompositeViewModel) as ViewResult;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task WhenBodyCalledWithInvalidPassword_ReturnToViewWithError()
        {
            var customer = new Customer
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                Contact = new Contact
                {
                    EmailAddress = "user"
                }
            };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).ReturnsForAnyArgs(customer);
            _openIdConnectClient.VerifyPassword("user","password").ReturnsForAnyArgs(Result.Fail("Failed"));
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient, _documentService, _config);
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body(closeYourAccountCompositeViewModel) as ViewResult;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
            result.ViewData.ModelState["Password"].Errors[0].ErrorMessage.Should().Be("Wrong password. Try again.");
        }

        [Test]
        public async Task WhenBodyCalled_RedirectToConfirmDelete()
        {
            var customer = new Customer
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                Contact = new Contact
                {
                    EmailAddress = "user"
                }
            };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).ReturnsForAnyArgs(customer);
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient, _documentService, _config);
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var result = await controller.Body(closeYourAccountCompositeViewModel) as ViewResult;
            result.Should().BeOfType<ViewResult>();
            result.ViewName = "ConfirmDeleteAccount";
            
        }
    }
}
