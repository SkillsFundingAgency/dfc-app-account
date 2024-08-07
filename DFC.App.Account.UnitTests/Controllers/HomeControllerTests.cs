﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using FakeItEasy;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<HomeController> _logger;
        private IAuthService _authService;
        private ISkillsHealthCheckService _skillsHealthCheckService;
        private IOptions<AuthSettings> _authSettings;
        private IOptions<ActionPlansSettings> _actionPlansSettings;
        private IDssReader _dssReader;
        private HomeController _controller;
        private IConfiguration _config;
        private ISharedContentRedisInterface _sharedContentRedisInterface;

        [SetUp]
        public void Init()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = Substitute.For<ILogger<HomeController>>();
            _authService = Substitute.For<IAuthService>();
            _skillsHealthCheckService = Substitute.For<ISkillsHealthCheckService>();
            _sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
            _dssReader = Substitute.For<IDssReader>();
            _authSettings = Options.Create(new AuthSettings
            {
                RegisterUrl = "reg",
                SignInUrl = "signin",
                SignOutUrl = "signout"
            });
            _actionPlansSettings = Options.Create(new ActionPlansSettings() {Url ="/actionj-plans"});
            _controller = new HomeController(_logger,_compositeSettings, _authService, _dssReader, _skillsHealthCheckService, _authSettings, _actionPlansSettings, _config, _sharedContentRedisInterface);
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = _controller.Head() as ViewResult;
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
            var customer = new Customer()
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                FamilyName = "familyName",
                GivenName = "givenName"
            };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new HomeController(_logger, _compositeSettings, _authService, _dssReader, _skillsHealthCheckService, _authSettings, _actionPlansSettings, _config, _sharedContentRedisInterface);
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

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result =  _controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
       
        [Test]
        public void WhenBodyFooterCalled_ReturnHtml()
        {
            var result = _controller.BodyFooter() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenErrorCalled_ReturnHtml()
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = _controller.Error() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void When_RegisterCalled_Return_Redirect()
        {
            var result = _controller.Register();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be(_authSettings.Value.RegisterUrl + "?redirecturl=/your-account");
        }

        [Test]
        public void When_SignInCalled_Return_Redirect()
        {
            var result = _controller.SignIn();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be(_authSettings.Value.SignInUrl + "?redirecturl=/your-account");
        }

        [Test]
        public async Task When_SignOutCalledWithAccountClosed_Return_Redirect()
        {
            var result = _controller.SignOut(true);
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be($"signout?redirectUrl={_authSettings.Value.Issuer}/your-account/Delete-Account/AccountClosed");
        }

        [Test]
        public async Task When_SignOutCalled_Return_Redirect()
        {
            var result = _controller.SignOut(false);
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("signout");
        }

    }
}
