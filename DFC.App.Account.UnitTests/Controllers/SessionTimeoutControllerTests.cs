using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using DFC.App.Account.Services;
using NSubstitute;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class SessionTimeoutControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IOptions<AuthSettings> _authSettings;


        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new SessionTimeoutController(_compositeSettings, _authService,_authSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
