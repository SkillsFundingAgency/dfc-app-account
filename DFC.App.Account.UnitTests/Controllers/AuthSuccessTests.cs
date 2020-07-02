using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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


namespace DFC.App.Account.UnitTests.Controllers
{
    public class AuthSuccessTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        
        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
        }

        [Test]
        public void When_RedirectCalled_Redirected()
        {
            var controller = new AuthSuccess(_compositeSettings, _authService);
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
