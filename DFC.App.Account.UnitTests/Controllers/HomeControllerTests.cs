using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Models;
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
        private IOptions<AuthSettings> _authSettings;

        [SetUp]
        public void Init()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = Substitute.For<ILogger<HomeController>>();
            _authService = Substitute.For<IAuthService>();
            _skillsHealthCheckService = Substitute.For<ISkillsHealthCheckService>();

            _authSettings = Options.Create(new AuthSettings
            {
                RegisterUrl = "reg",
                SignInUrl = "signin",
                SignOutUrl = "signout"
            });
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
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
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
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
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public async Task WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public async Task WhenBodyTopCalled_ReturnHtmlWithUserName()
        {
            var customer = new Customer()
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                FamilyName = "familyName",
                GivenName = "givenName"
            };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    HttpContext = { User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("CustomerId", "test")
                    },"testType"))}
                }
            };
            var result = await controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as CompositeViewModel;
            model.Name.Should().Be("givenName familyName");
        }
        [Test]
        public void WhenBodyFooterCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            var result = controller.BodyFooter() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenErrorCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
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
        public void When_RegisterCalled_Return_Redirect()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            var result = controller.Register();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be(_authSettings.Value.RegisterUrl + "?redirecturl=/your-account");
        }

        [Test]
        public void When_SignInCalled_Return_Redirect()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            var result = controller.SignIn();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be(_authSettings.Value.SignInUrl + "?redirecturl=/your-account");
        }

        [Test]
        public async Task When_SignOutCalled_Return_Redirect()
        {
            var controller = new HomeController(_logger, _compositeSettings, _authService, _skillsHealthCheckService, _authSettings);
            var result = controller.SignOut("test");
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("signout?redirectUrl=test");
        }


    }
}
