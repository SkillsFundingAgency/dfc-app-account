using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using FakeItEasy;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class AuthSuccessTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IDssWriter _dssWriter;
        private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;
        private ISharedContentRedisInterface _sharedContentRedisInterface;


        [SetUp]
        public void Init()
        {
            _documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            _sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            _compositeSettings = Options.Create(new CompositeSettings());

            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = Substitute.For<IAuthService>();
            _dssWriter = Substitute.For<IDssWriter>();
            _sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
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
            var controller = new AuthSuccess( _compositeSettings, _authService, _dssWriter, _config, _sharedContentRedisInterface);
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

            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();

        }
        [Test]
        public void When_RedirectCalled_Redirected()
        {
            var controller = new AuthSuccess(_compositeSettings, _authService,_dssWriter, _config, _sharedContentRedisInterface);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Redirect("/home") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();

            result.Url.Should().Be("/home");
        }
    }
}
