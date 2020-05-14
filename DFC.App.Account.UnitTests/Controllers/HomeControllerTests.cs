using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<HomeController> _logger;
        private IAuthService _authService;
        private ISkillsHealthCheckService _skillsHealthCheckService;
        private IOpenIDConnectClient _authClient;

        [SetUp]
        public void Init()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = Substitute.For<ILogger<HomeController>>();
            _authService = Substitute.For<IAuthService>();
            _skillsHealthCheckService = Substitute.For<ISkillsHealthCheckService>();
            
            _authClient = Substitute.For<IOpenIDConnectClient>();
            _authClient.GetRegisterUrl().ReturnsForAnyArgs("http://register");
            _authClient.GetSignInUrl().ReturnsForAnyArgs("http://signin");
            _authClient.GetAuthdUrl().ReturnsForAnyArgs("http://homepage");
            _authClient.ValidateToken("badtoken").Throws(new System.Exception("Invalid token"));
            _authClient.ValidateToken("goodtoken").Returns(Task.FromResult(new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(claims: new Claim[] { new Claim("name", "Mr Test User") } )));
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Head() as ViewResult;
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title",

            };
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
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
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBodyFooterCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = controller.BodyFooter() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenErrorCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Error() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void AssigningViewModelValues()
        {
            var viewModel = new HomeCompositeViewModel();
            viewModel.ShcDocuments.Add(new ShcDocument());
            var item = viewModel.ShcDocuments[0];
            item.Should().NotBeNull();
        }

        [Test]
        public async Task When_RegisterCalled_Return_Redirect()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = await controller.Register();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("http://register");
        }

        [Test]
        public async Task When_SignInCalled_Return_Redirect()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = await controller.SignIn();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("http://signin");
        }

        [Test]
        public async Task When_AuthCalledWithInvalidToken_ThrowException()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            Func<Task> act = async () => { await controller.Auth("badtoken", "state"); };
            await act.Should().ThrowAsync<System.Exception>();
        }

        [Test]
        public async Task When_AuthCalledWithValidToken_Return_RedirectWithTemporaryUsernameClaim()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authClient);
            var result = await controller.Auth("goodtoken", "state");
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("http://homepage?UserFullName=Mr Test User");
        }
    }
}
