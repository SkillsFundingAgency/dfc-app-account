using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.Account.Application.Common.Services;
using NUnit.Framework;

namespace DFC.App.Account.Application.UnitTests
{
    public class ServiceFunctionTests
    {

        [Test]
        public void When_IsRegexCalledWithEmptyString_ReturnFalse()
        {
            var result = ServiceFunctions.IsValidRegexValue("", "");
            Assert.False(result);
        }


        [Test]
        public void When_IsRegexCalledWithValidString_ReturnTrue()
        {
            var result = ServiceFunctions.IsValidRegexValue("07867565456", "^(\\+44\\s?7\\d{3}|\\(?07\\d{3}\\)?)\\s?\\d{3}\\s?\\d{3}$");
            Assert.True(result);
        }
    }
}
