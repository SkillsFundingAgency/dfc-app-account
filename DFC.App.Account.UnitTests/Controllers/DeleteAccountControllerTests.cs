using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
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

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new DeleteAccountController(_compositeSettings, _authService);
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
        public async Task WhenBodyCalled_AccountDeleted()
        {
            var controller = new DeleteAccountController(_compositeSettings, _authService);
            var deleteAccountCompositeViewModel = new DeleteAccountCompositeViewModel();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var result =  await controller.Body(deleteAccountCompositeViewModel) as ViewResult;
            result.Should().BeOfType<ViewResult>();
            result.ViewName = "AccountDeleted";
            
        }

        
    }
}
