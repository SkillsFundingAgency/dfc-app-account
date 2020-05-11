using System;
using DFC.App.Account.Models;
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