using DFC.App.Account.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;
using DFC.App.Account.ViewModels;

namespace DFC.App.MatchSkills.Controllers
{
//    [ApiController]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public HealthController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        [Route("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpGet]
        [Route("EndPoints")]
        public IActionResult EndPoints()
        {
            var actionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors.Items;
            var model = new EndPointsViewModel();
            model.EndPoints = actionDescriptors
                .Select(ad => new EndPoint()
                {
                    Action = ad.RouteValues["action"],
                    Controller = ad.RouteValues["controller"],
                    Methods = string.Join(", ", ad.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? new string[] { "GET" }),
                }).ToList();
            return View(model);
        }
    }
}