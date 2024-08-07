﻿using System;
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
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using FakeItEasy;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<ErrorController> _logger;
        private IAuthService _authService;
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

            _logger = Substitute.For<ILogger<ErrorController>>();
            _sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = new Logger<ErrorController>(new LoggerFactory());
            _authService = Substitute.For<IAuthService>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new ErrorController(_logger, _compositeSettings, _authService, _config, _sharedContentRedisInterface);
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
