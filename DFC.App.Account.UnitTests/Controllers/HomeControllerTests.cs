using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.Account.UnitTests
{
    public class HomeControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<HomeController> _logger;
         
        
        [SetUp]
        public void Init()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = Substitute.For<ILogger<HomeController>>();
        }
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings );
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Head() as ViewResult;
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title",
                
            };
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings );
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_logger,_compositeSettings );
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        
    }
}
