using DFC.App.Account.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.Compui.Cosmos.Enums;
using System.Net.Mime;

namespace DFC.App.Account.Controllers
{
    public class SitemapController : Controller
    {

        private readonly ILogger<SitemapController> logger;

        public SitemapController(ILogger<SitemapController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("pages/sitemap")]
        public IActionResult SitemapView()
        {
            return Sitemap();
        }

        [HttpGet]
        [Route("/sitemap.xml")]
        public IActionResult Sitemap()
        {
            try
            {
                logger.LogInformation("Generating Sitemap");

                var sitemapUrlPrefix = $"{Request.GetBaseAddress()}".TrimEnd('/');
                var sitemap = new Sitemap();
                sitemap.Add(new SitemapLocation
                {
                    Url = $"{sitemapUrlPrefix}/your-account",
                    Priority = 0.5,
                    ChangeFrequency = SiteMapChangeFrequency.Weekly,
                });

                var xmlString = sitemap.WriteSitemapToString();
                logger.LogInformation("Generated Sitemap");

                return Content(xmlString, MediaTypeNames.Application.Xml);
            }
#pragma warning disable CA1031
            catch (System.Exception ex)
#pragma warning restore CA1031
            {
                logger.LogError(ex, "{NameofSitemap}: {ExMessage}", nameof(Sitemap), ex.Message);
            }

            return BadRequest();
        }
    }
}
