using System.Collections.Generic;
using DFC.App.Account.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Controllers
{
  
        public class HealthControllerTests
        {
            [Test]
            public void Ping()
            {
                // Arrange
                var controller = new HealthController(null);

                // Act
                var result = controller.Ping();

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<OkResult>();
            }

            [Test]
            public void EndPoints()
            {
                // Arrange
                var actionDescriptorCollectionMock = new Mock<IActionDescriptorCollectionProvider>();
                var actionActionDescriptors = new List<ActionDescriptor>();
                var routeValues = new Dictionary<string, string>()
                {
                    {"controller", "controllerName"},
                    {"action", "actionName"}
                };

                actionActionDescriptors.Add(new ActionDescriptor(){RouteValues = routeValues});
                actionDescriptorCollectionMock.Setup(m => m.ActionDescriptors).Returns(new ActionDescriptorCollection(actionActionDescriptors, 0));
                var controller = new HealthController(actionDescriptorCollectionMock.Object);

                // Act
                var result = controller.EndPoints();

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<ViewResult>();
            }
        }

    
}
