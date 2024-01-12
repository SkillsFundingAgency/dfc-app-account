using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<ErrorController> _logger;
        private IAuthService _authService;
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

            _logger = Substitute.For<ILogger<ErrorController>>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = new Logger<ErrorController>(new LoggerFactory());
            _authService = Substitute.For<IAuthService>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new ErrorController(_logger, _compositeSettings, _authService, _config);
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
