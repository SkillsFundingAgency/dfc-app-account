using System;
using System.Threading.Tasks;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Models
{
    class GenericIdentityRequestTests
    {
        [Test]
        public void CreateGenericIdentityRequest()
        {
            var genericIdentityRequest = new GenericIdentityRequest();
            genericIdentityRequest.CitizenId = new Guid();
        }
    }
}