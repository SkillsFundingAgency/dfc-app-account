using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.SHC.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;


namespace DFC.App.Account.UnitTests.Controllers
{
    public class AuthSuccessTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IDssWriter _dssWriter;

        
        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _dssWriter = Substitute.For<IDssWriter>();
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
            var controller = new AuthSuccess( _compositeSettings, _authService, _dssWriter);
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
            var controller = new AuthSuccess(_compositeSettings, _authService,_dssWriter);
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
