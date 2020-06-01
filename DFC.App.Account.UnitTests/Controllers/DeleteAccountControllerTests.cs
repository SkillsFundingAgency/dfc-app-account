using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class DeleteAccountControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IDssWriter _dssService;

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _dssService = Substitute.For<IDssWriter>();
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new DeleteAccountController(_compositeSettings,_dssService, _authService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();

            result.Url.Should().Be("~/home");
        }


        [Test]
        public async Task WhenBodyCalled_AccountDeleted()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new DeleteAccountController(_compositeSettings, _dssService,_authService);


            var deleteAccountCompositeViewModel = new DeleteAccountCompositeViewModel
            {
                CustomerId = Guid.Parse("c2e27821-cc60-4d3d-b4f0-cbe20867897c")
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var result =  await controller.Body(deleteAccountCompositeViewModel) as ViewResult;
            result.Should().BeOfType<ViewResult>();
            result.ViewName = "AccountDeleted";
            
        }

        [Test]
        public async Task WhenPostBodyCalledAndModelIdDoesNotMatchLoggedInId_ThenRedirectToHome()
        {
            var customer = new Customer() { CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c") };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new DeleteAccountController(_compositeSettings, _dssService, _authService);

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
