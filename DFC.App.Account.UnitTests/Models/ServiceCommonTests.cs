using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.ViewModels;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Models
{
    public class ServiceCommonTests
    {
        [Test]
        public void GetDisplayNameReturnsCorrectValue()
        {
            var model = new YourDetailsCompositeViewModel();
            model.CustomerDetails.Gender = CommonEnums.Gender.Female;
            Assert.True(CommonEnums.Gender.Female.ToString() == model.CustomerDetails.Gender.GetDisplayName());
        }

        [Test]
        public void GetDisplayReturnValueIfEnumNotFound()
        {
            var model = new YourDetailsCompositeViewModel();
            Assert.True(model.CustomerDetails.Gender.GetDisplayName() == "0");
        }
    }
}
