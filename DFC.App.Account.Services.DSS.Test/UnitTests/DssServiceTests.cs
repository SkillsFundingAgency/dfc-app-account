using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using DFC.App.Account.Services.DSS.UnitTests.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Services.DSS.UnitTests.UnitTests
{
    public class CreateCustomerDataTests
    {
        private IDssWriter _dssService;
        private RestClient _restClient;
        [SetUp]
        public void Setup()
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }));
        }

        [Test]
        public async Task IfCustomerIsNull_ReturnNull()
        {
            _dssService = new DssService(Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }));
            var result = await _dssService.CreateCustomerData(null);
            result.Should().BeNull();
        }
        [Test]
        public async Task IfApiCallIsSuccessful_ReturnCreatedCustomerData()
        {
            var result = await _dssService.CreateCustomerData(new Customer());
            result.Should().NotBeNull();
        }
        [Test]
        public async Task IfApiCallIsUnSuccessful_ReturnNull()
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.InternalServerError);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }));

            var result = await _dssService.CreateCustomerData(new Customer());
            result.Should().BeNull();
        }

    }
}