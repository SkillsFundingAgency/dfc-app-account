using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _openIdConnectClient = Substitute.For<IOpenIDConnectClient>();
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient);
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
        public void WhenBodyCalledWithInvalidModelState_ReturnToViewWithError()
        {
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient);
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ModelState.AddModelError("password","Invalid password");
            var result =  controller.Body(closeYourAccountCompositeViewModel) as ViewResult;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenBodyCalled_RedirectToConfirmDelete()
        {
            var controller = new CloseYourAccountController(_compositeSettings, _authService,_openIdConnectClient);
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var result =  controller.Body(closeYourAccountCompositeViewModel) as ViewResult;
            result.Should().BeOfType<ViewResult>();
            result.ViewName = "ConfirmDeleteAccount";
            
        }
    }
}
