using System;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
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
    class YourDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IDssReader _dssService;
        private ILogger<YourDetailsController> _logger;
        private YourDetailsController _controller;
        private Customer _customer;
        private IAuthService _authService;
        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _dssService = Substitute.For<IDssReader>();
            _authService = Substitute.For<IAuthService>();
            _customer = new Customer()
            {
                Addresses = new []{new Address() {Address1 = "Line 1"}},
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
            _dssService.GetCustomerData(Arg.Any<string>()).Returns(_customer);
            _controller = new YourDetailsController(_logger, _compositeSettings, _dssService, _authService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = await _controller.Body("somecustomerid") as ViewResult;
            result.ViewData.Model.As<YourDetailsCompositeViewModel>().CustomerDetails.Should().NotBeNull();
        }


    }
}
