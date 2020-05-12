using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Services
{
    public class GetAddressIOSearchServiceTests
    {
        private IRestClient _restClient;

        public GetAddressIOSearchServiceTests()
        {
            _restClient = Substitute.For<IRestClient>();

            _restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = true
            };
        }

        [Test]
        public async Task WhenGetAddressesCalled_ThenCorrectUrlIsGenerated()
        {
            _restClient.GetAsync<GetAddressIoResult>(Arg.Any<string>())
                .ReturnsForAnyArgs(new GetAddressIoResult
                {
                    Addresses = new List<GetAddressIOAddressModel>()
                });

            var service = new GetAddressIoSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings
            {
                Key = "1",
                AddressIdentifierPattern = "2",
                FindAddressesBaseUrl = "3",
                RetrieveAddressBaseUrl = "4"
            }), _restClient);

            await service.GetAddresses("test");

            await _restClient.Received().GetAsync<GetAddressIoResult>(Arg.Is("3/test?api-key=1&expand=true"));
        }

        [Test]
        public async Task WhenGetAddressesCalled_ThenReturnResult()
        {
            _restClient.GetAsync<GetAddressIoResult>(Arg.Any<string>())
                .ReturnsForAnyArgs(new GetAddressIoResult
                {
                    Addresses = new List<GetAddressIOAddressModel>()
                });

            _restClient.LastResponse.IsSuccess = true;

            var service = new GetAddressIoSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddresses("test");

            result.Should().BeOfType<List<PostalAddressModel>>();

        }

        [Test]
        public async Task WhenGetAddressesCalledAndNoAddressesFound_ThenReturnNull()
        {
            _restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = false
            };

            _restClient.GetAsync<IEnumerable<PostalAddressModel>>(Arg.Any<string>())
                .ReturnsForAnyArgs(new List<PostalAddressModel>());

            var service = new GetAddressIoSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddresses("test");

            result.Should().BeNull();

        }

        [Test]
        public void WhenGetAddressCalledAndNoAddressFound_ThenReturnNull()
        {
            _restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = false
            };

            _restClient.GetAsync<string>(Arg.Any<string>())
                .ReturnsForAnyArgs(JsonConvert.SerializeObject(new List<PostalAddressModel>()));

            var service = new GetAddressIoSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = service.GetAddress("test");

            result.Should().BeNull();

        }
    }
}
