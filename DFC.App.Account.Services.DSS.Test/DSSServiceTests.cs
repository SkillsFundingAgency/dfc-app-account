using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.Services.DSS.UnitTests
{
    public class CreateCustomerDataTests
    {
        private IDssWriter _dssService;
        private RestClient _restClient;
        [SetUp]
        public void Setup()
        {
            _restClient = Substitute.For<RestClient>();
            _dssService = new DssService(_restClient, new DssSettings()
            {
                ApiUrl = "Url",
                ApiKey = "Key"
            });
        }

        [Test]
        public async Task IfCustomerIsNull_ReturnNull()
        {
            var result = await _dssService.CreateCustomerData(null);
            result.Should().BeNull();
        }
        [Test]
        public async Task IfApiCallIsSuccessful_ReturnHttpResponseMessage()
        {
            _restClient.PostAsync<HttpResponseMessage>(apiPath: Arg.Any<string>(), content: Arg.Any<HttpContent>(), ocpApimSubscriptionKey: Arg.Any<string>())
                .Returns(new HttpResponseMessage(HttpStatusCode.Created));
            var result = await _dssService.CreateCustomerData(new Customer());
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}