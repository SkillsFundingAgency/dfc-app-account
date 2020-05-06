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
    class CloseYourAccountCompositeViewModelTests
    {
        [Test]
        public void CloseYourAccountCompositeViewModel()
        {
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();
            closeYourAccountCompositeViewModel.Password = "Password";
            closeYourAccountCompositeViewModel.Reason = "Reason";
        }
    }
}