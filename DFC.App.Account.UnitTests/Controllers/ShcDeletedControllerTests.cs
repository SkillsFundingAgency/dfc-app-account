using DFC.App.Account.Application.SkillsHealthCheck.Models;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class ShcDeletedControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private ISkillsHealthCheckService _skillsHealthCheckService;
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
            _skillsHealthCheckService = Substitute.For<ISkillsHealthCheckService>();
        }
        [Test]
        public async Task WhenDefaultBodyCalled_ReturnHtml()
        {
            var controller = new ShcDeletedController(_compositeSettings, _authService, _skillsHealthCheckService, _config);
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
        public async Task WhenBodyCalledWithoutId_ReturnHomeHtml()
        {
            var controller = new ShcDeletedController(_compositeSettings, _authService, _skillsHealthCheckService, _config);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body("") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();

            result.Url.Should().Be("~/home");
        }
        [Test]
        public async Task WhenShcDocumentsReturnEmpty_ReturnHomeHtml()
        {
            var controller = new ShcDeletedController(_compositeSettings, _authService, _skillsHealthCheckService, _config);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _skillsHealthCheckService.GetShcDocumentsForUser(Arg.Any<string>()).Returns(new List<ShcDocument>());
            var result = await controller.Body("123345") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();

            result.Url.Should().Be("~/home");
        }
        [Test]
        public async Task WhenShcDocumentForIdNotFound_ReturnHomeHtml()
        {
            var controller = new ShcDeletedController(_compositeSettings, _authService, _skillsHealthCheckService, _config);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _skillsHealthCheckService.GetShcDocumentsForUser(Arg.Any<string>()).Returns(new List<ShcDocument> { new ShcDocument() });
            var result = await controller.Body("123345") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();

            result.Url.Should().Be("~/home");
        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new ShcDeletedController(_compositeSettings, _authService, _skillsHealthCheckService, _config);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _skillsHealthCheckService.GetShcDocumentsForUser(Arg.Any<string>()).Returns(new List<ShcDocument>
            {
                new ShcDocument
                {
                    CreatedAt = DateTimeOffset.UtcNow,
                    LinkUrl = "url", DocumentId = "200010216"
                }
            });
            var result = await controller.Body("200010216") as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            result.ViewName.Should().BeNull();
        }

    }
}
