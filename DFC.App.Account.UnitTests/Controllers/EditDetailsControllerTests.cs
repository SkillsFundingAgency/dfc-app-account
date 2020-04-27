using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class EditDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;


        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new EditDetailsController(_compositeSettings);
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
