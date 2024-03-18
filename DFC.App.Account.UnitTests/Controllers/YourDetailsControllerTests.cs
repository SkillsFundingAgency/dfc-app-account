using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
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
    class YourDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IDssReader _dssService;
        private ILogger<YourDetailsController> _logger;
        private YourDetailsController _controller;
        private Customer _customer;
        private IAuthService _authService;
       //private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;
        private ISharedContentRedisInterface _sharedContentRedisInterface;
        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _dssService = Substitute.For<IDssReader>();
            _authService = Substitute.For<IAuthService>();
            //_documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            _sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();


            _customer = new Customer()
            {
                Contact = new Contact() {ContactId = "id",EmailAddress = "email"},
                OptInMarketResearch = true,
                OptInUserResearch = true,
                GivenName = "First Name",
                CustomerId = Guid.Parse("a9dacf19-d11f-45b6-a958-8a4cf24f1407"),
                FamilyName = "Surname"
            };
        }

       

        [Test]
        public async Task WhenBodyCalled_ReturnCustomer()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            _dssService.GetCustomerData(Arg.Any<string>()).Returns(_customer);
            _controller = new YourDetailsController(_logger, _compositeSettings, _dssService, _authService, _config, _sharedContentRedisInterface);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = await _controller.Body() as ViewResult;
            result.ViewData.Model.As<YourDetailsCompositeViewModel>().CustomerDetails.Should().NotBeNull();
        }


    }
}
