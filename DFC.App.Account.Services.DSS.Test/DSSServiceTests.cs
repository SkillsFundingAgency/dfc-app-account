using System.Threading.Tasks;
using DFC.App.Account.Services.DSS;
using DFC.App.Account.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using NUnit.Framework;

namespace DFC.App.Account.Services.DSS.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task When_GetCustomer_Return_Customer () 
        {
            var _restClient = new RestClient();
            var sut = new DSSService(_restClient);

            var result = await sut.GetCustomer("f72c07d6-e3a6-4dc2-9e62-2e91f09e484e", "9000000000");

        }
    }
}