using System;
using System.Collections.Generic;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Services;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class SessionTimeoutControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IOptions<AuthSettings> _authSettings;
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
            _authSettings = Options.Create(new AuthSettings()
            {
                SignInUrl = "",
                SignOutUrl = ""
            });

        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new SessionTimeoutController(_compositeSettings, _authService,_authSettings, _config);
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
