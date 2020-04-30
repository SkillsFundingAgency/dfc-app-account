﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Services
{
    public class AuthServiceTests
    {
        private IDssReader _dssService;

        public AuthServiceTests()
        {
            _dssService = Substitute.For<IDssReader>();
        }

        [Test]
        public async Task WhenAuthServiceCalledReturnCustomer()
        {
            _dssService.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer()
            {
                FamilyName = "Test"
            });

            var service = new AuthService(_dssService);

            var result = await service.GetCustomer(new ClaimsPrincipal());

            result.FamilyName.Should().Be("Test");
        }
    }
}