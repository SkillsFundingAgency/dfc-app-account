using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly IDssReader _dssService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthServiceTests()
        {
            _dssService = Substitute.For<IDssReader>();
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        }

        [Test]
        public async Task WhenAuthServiceCalledReturnCustomer()
        {
            _dssService.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer()
            {
                FamilyName = "Test"
            });

            var service = new AuthService(_dssService,_httpContextAccessor);

            var result = await service.GetCustomer(new ClaimsPrincipal());

            result.FamilyName.Should().Be("Test");
        }

        [Test]
        public void AuthSettingsTest()
        {
            var auth = new AuthSettings
            {
                SignOutUrl = "t",
                SignInUrl = "t",
                ClientId = "t",
                ClientSecret = "t",
                Issuer = "t",
                RegisterUrl = "t",
            };

            auth.SignOutUrl.Should().NotBeNullOrEmpty();
            auth.SignInUrl.Should().NotBeNullOrEmpty();
            auth.ClientId.Should().NotBeNullOrEmpty();
            auth.ClientSecret.Should().NotBeNullOrEmpty();
            auth.Issuer.Should().NotBeNullOrEmpty();
            auth.RegisterUrl.Should().NotBeNullOrEmpty();
        } 
    }
}
