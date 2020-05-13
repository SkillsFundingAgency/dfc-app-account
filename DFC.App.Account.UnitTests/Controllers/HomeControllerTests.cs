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
    }
}
