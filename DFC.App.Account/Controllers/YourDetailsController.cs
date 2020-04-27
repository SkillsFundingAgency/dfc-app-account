﻿using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class YourDetailsController : CompositeSessionController<YourDetailsCompositeViewModel>
    {
        public YourDetailsController(IOptions<CompositeSettings> compositeSettings) : base(compositeSettings)
        {
            
        }
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}