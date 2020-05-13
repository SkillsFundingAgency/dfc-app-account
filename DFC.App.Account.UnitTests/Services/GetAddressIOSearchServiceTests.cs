using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Exception;
using DFC.App.Account.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

        [Test]
        public async Task WhenGetAddressesCalledAndServiceLimitReached_ThenThrowServiceLimitReachedException()
        {
            var client = Substitute.For<IRestClient>();

            client.GetAsync<GetAddressIoResult>(Arg.Any<string>())
                .ReturnsForAnyArgs(new GetAddressIoResult
                {
                    Addresses = new List<GetAddressIOAddressModel>()
                });

            client.LastResponse = new RestClient.APIResponse(new HttpResponseMessage())
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.TooManyRequests
            };

            var service = new GetAddressIoSearchService(new OptionsWrapper<AddressSearchServiceSettings>(new AddressSearchServiceSettings()), client);

            Assert.ThrowsAsync<AddressServiceRequestLimitReachedException>(async () =>
            {
                await service.GetAddresses("test");
            });

        }

        [Test]
        public void GetAddressViewModelTest()
        {
            var model = new GetAddressIOAddressModel{
                Line1 = "",
                City = "",
                Line3 = "",
                Line4 = "",
                Line2 = "",
                County = "",
                FormattedAddress = new List<string>()
            };

            model.Should().NotBeNull();
        }

        [Test]
        public void GetAddressResultModelTest()
        {
            var model = new GetAddressIoResult()
            {
                Addresses = new List<GetAddressIOAddressModel>{
                new GetAddressIOAddressModel
                {
                    FormattedAddress = new List<string>()
                }

                },
                Latitude = "",
                Longitude = "",
                Postcode = nameof(GetAddressIoResult.Postcode)
            };

            model.MapToPostalAddressModel().First().PostalCode.Should().Be(nameof(GetAddressIoResult.Postcode));
        }
    }
}
