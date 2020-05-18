using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.Account.UnitTests.Services
{
    public class AddressSearchServiceTests
    {

        private IRestClient _restClient;

        public AddressSearchServiceTests()
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
            _restClient.GetAsync<IEnumerable<PostalAddressModel>>(Arg.Any<string>())
                .ReturnsForAnyArgs(new List<PostalAddressModel>());

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings
            {
                Key = "1",
                AddressIdentifierPattern = "2",
                FindAddressesBaseUrl = "3",
                RetrieveAddressBaseUrl = "4"
            }), _restClient);

            await service.GetAddresses("test");

            await _restClient.Received().GetAsync<IEnumerable<PostalAddressModel>>(Arg.Is("3&Key=1&SearchTerm=test&LastId=&SearchFor=PostalCodes&Country=GBR&LanguagePreference=EN&MaxSuggestions=&MaxResults="));
        }

        [Test]
        public async Task WhenGetAddressCalled_ThenCorrectUrlIsGenerated()
        {
            _restClient.GetAsync<IEnumerable<PostalAddressModel>>(Arg.Any<string>())
                .ReturnsForAnyArgs(new List<PostalAddressModel>());

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings
            {
                Key = "1",
                AddressIdentifierPattern = "2",
                FindAddressesBaseUrl = "3",
                RetrieveAddressBaseUrl = "4"
            }), _restClient);

            await service.GetAddress("test");

            await _restClient.Received().GetAsync<IEnumerable<PostalAddressModel>>(Arg.Is("4&Key=1&Id=test"));
        }

        [Test]
        public async Task WhenGetAddressesCalled_ThenReturnResult()
        {
            var client = Substitute.For<IRestClient>();

            client.GetAsync<IEnumerable<PostalAddressModel>>(Arg.Any<string>())
                .ReturnsForAnyArgs(new List<PostalAddressModel>());

            client.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = true
            };

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), client);

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

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddresses("test");

            result.Should().BeNull();

        }

        [Test]
        public async Task WhenGetAddressCalled_ThenReturnResult()
        {
            _restClient.GetAsync<IEnumerable<PostalAddressModel>>(Arg.Any<string>())
                .ReturnsForAnyArgs(new List<PostalAddressModel>
                {
                    new PostalAddressModel
                    {
                        Id = "test"
                    }
                });

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddress("test");

            result.Should().BeOfType<PostalAddressModel>();

        }

        [Test]
        public async Task WhenGetAddressCalledAndNoAddressFound_ThenReturnNull()
        {
            _restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = false
            };

            _restClient.GetAsync<string>(Arg.Any<string>())
                .ReturnsForAnyArgs(JsonConvert.SerializeObject(new List<PostalAddressModel>()));

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddress("test");

            result.Should().BeNull();

        }

        [Test]
        public async Task WhenGetAddressesCalledAndCallFails_ThenReturnNull()
        {
            _restClient.GetAsync<string>(Arg.Any<string>())
                .ReturnsForAnyArgs(string.Empty);

            var service = new AddressSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), _restClient);

            var result = await service.GetAddress("test");

            result.Should().BeNull();

        }
    }
}
