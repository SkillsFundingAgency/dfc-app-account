using System;
using System.Collections.Generic;
using System.Linq;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using DFC.App.Account.Services.DSS.UnitTests.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace DFC.App.Account.Services.DSS.UnitTests.UnitTests
{
    public class CreateCustomerDataTests
    {
        private IDssWriter _dssService;
        private RestClient _restClient;
        private ILogger<DssService> _logger;
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For <ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }), _logger);
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
            }), _logger);
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
            }), _logger);

            var result = await _dssService.CreateCustomerData(new Customer());
            result.Should().BeNull();
        }

       
    }

    public class GetCustomerDataTests
    {
        private IDssReader _dssService;
        private RestClient _restClient;
        private IOptions<DssSettings> _dssSettings;
        private ILogger<DssService> _logger;
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssSettings = Options.Create(new DssSettings()
            {
                ApiKey = "0000dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk/api/Customers/",
                AccountsTouchpointId = "9000000000",
                CustomerApiVersion = "V2",
                CustomerAddressDetailsApiUrl = "https://this.is.anApi.org.uk/api/Customers/{customerId}/Addresses",
                CustomerAddressDetailsApiVersion = "V2",
                CustomerContactDetailsApiUrl = "https://this.is.anApi.org.uk/customers/{customerId}/ContactDetails/",
                CustomerContactDetailsApiVersion = "V2"
            });
        }
        [Test]
        public async Task When_GetCustomerDetail_Return_Customer () 
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.OK);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);
           var result = await _dssService.GetCustomerDetail("993cfb94-12b7-41c4-b32d-7be9331174f1",new HttpRequestMessage());
           result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetCustomerContactDetails_Return_Contact () 
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerContactDetails(), statusToReturn: HttpStatusCode.OK);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);
            var result = await _dssService.GetCustomerContactDetails("993cfb94-12b7-41c4-b32d-7be9331174f1",new HttpRequestMessage());
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetCustomerAddressDetails_Return_Address () 
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerAddressDetails(), statusToReturn: HttpStatusCode.OK);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);
            var result = await _dssService.GetCustomerAddressDetails("993cfb94-12b7-41c4-b32d-7be9331174f1",new HttpRequestMessage());
            result.Should().NotBeNull();
        }


        [Test]
        public async Task When_GetCustomerAddressDetails_Return_NullAddress () 
        {
            var mockHandler = DssHelpers.GetMockMessageHandler("", statusToReturn: HttpStatusCode.NoContent);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);
            var result = await _dssService.GetCustomerAddressDetails("993cfb94-12b7-41c4-b32d-7be9331174f1",new HttpRequestMessage());
            result.Should().BeNull();
        }

        [Test]
        public async Task When_GetCustomerDetailWithNoData_Throw_DSSException () 
        {
            var mockHandler = DssHelpers.GetMockMessageHandler("&$a", statusToReturn: HttpStatusCode.InternalServerError);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);

            var result = await _dssService.GetCustomerAddressDetails("993cfb94-12b7-41c4-b32d-7be9331174f1", new HttpRequestMessage());

            _logger.Received(3);
            result.Should().BeNull();


        }
    }

    public class UpdateCustomerDataTests
    {
        private IDssWriter _dssService;
        private RestClient _restClient;
        private ILogger<DssService> _logger;
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(),
                statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }), _logger);
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
            }), _logger);
            var result = await _dssService.UpdateCustomerData(null);
            result.Should().BeNull();
        }
        [Test]
        public async Task IfApiCallIsSuccessful_ReturnCreatedCustomerData()
        {
            var result = await _dssService.UpdateCustomerData(new Customer());
            result.Should().NotBeNull();
        }
        [Test]
        public void IfApiCallIsUnSuccessful_ReturnNull()
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.InternalServerError);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }), _logger);

            _dssService.Invoking(x => x.UpdateCustomerData(new Customer())).Should()
                .Throw<UnableToUpdateCustomerDetailsException>();

        }

        [Test]
        public void When_DeleteCustomerRequestNull_ThrowException()
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.InternalServerError);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }), _logger);

            _dssService.Invoking(x => x.DeleteCustomer(Guid.NewGuid())).Should()
                .Throw<UnableToUpdateCustomerDetailsException>();

        }

        [Test]
        public void When_DeleteCustomerRequestEmptyGuid_ThrowException()
        {
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerCreation(), statusToReturn: HttpStatusCode.InternalServerError);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                AccountsTouchpointId = "9000000001",
                CustomerApiVersion = "V1"
            }), _logger);

            _dssService.Invoking(x => x.DeleteCustomer(Guid.Empty)).Should()
                .Throw<UnableToUpdateCustomerDetailsException>();

        }

        [Test]
        public async Task When_DeleteCustomerSuccess_ReturnOk()
        {
            await _dssService.DeleteCustomer(Guid.NewGuid());

        }
    }


    public class ActionPlanTest
    {
        private IDssReader _dssService;
        private RestClient _restClient;
        private IOptions<DssSettings> _dssSettings;
        private ILogger<DssService> _logger;
        
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerActionPlanDetails(), statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssSettings = Options.Create(new DssSettings()
            {
                ApiKey = "0000dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk/api/Customers/",
                AccountsTouchpointId = "9000000000",
                CustomerApiVersion = "V2",
                CustomerAddressDetailsApiUrl = "https://this.is.anApi.org.uk/api/Customers/{customerId}/Addresses",
                CustomerAddressDetailsApiVersion = "V2",
                CustomerContactDetailsApiUrl = "https://this.is.anApi.org.uk/customers/{customerId}/ContactDetails/",
                CustomerContactDetailsApiVersion = "V2",
                ActionPlansApiUrl="SomeAPI",
                ActionPlansApiVersion = "V2"
            });
        }

        [Test]
        public void When_GetActionPlans_Return_ActionPlans()
        {
            _dssService = new DssService(_restClient, _dssSettings, _logger);
            var result = _dssService.GetActionPlans("somecustomerid");
            result.Should().NotBe(null);
        }

        [Test]
        public void When_GetActionPlansWithException_Return_Exception()
        {
            var restClient = Substitute.For<IRestClient>();
            restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            

            restClient.GetAsync<IList<ActionPlan>>(Arg.Any<string>(),Arg.Any<HttpRequestMessage>()).Returns<IList<ActionPlan>>(x => { throw new DssException("Failure Action Plans");});
            
            _dssService = new DssService(restClient, _dssSettings, _logger);

            _dssService.Invoking(sut => sut.GetActionPlans("993cfb94-12b7-41c4-b32d-7be9331174f1"))
                .Should().Throw<DssException>();

        }
        [Test]
        public void When_GetActionPlansWithNoContent_Return_EmptyList()
        {
            var restClient = Substitute.For<IRestClient>();
            restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
            _dssService = new DssService(restClient, _dssSettings, _logger);
            var result = _dssService.GetActionPlans("somecustomerid");

            result.Result.Count.Should().Be(0);

        }
    }

    public class DigitalIdentitiesTests
    {
        private IDssWriter _dssService;
        private RestClient _restClient;
        private IOptions<DssSettings> _dssSettings;
        private ILogger<DssService> _logger;
        
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDigitalIdentitiesCustomerUpdateDetails(), statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssSettings = Options.Create(new DssSettings()
            {
                ApiKey = "0000dfjsjdsidfs83fds",
                CustomerApiUrl = "https://this.is.anApi.org.uk/api/Customers/",
                AccountsTouchpointId = "9000000000",
                CustomerApiVersion = "V2",
                CustomerAddressDetailsApiUrl = "https://this.is.anApi.org.uk/api/Customers/{customerId}/Addresses",
                CustomerAddressDetailsApiVersion = "V2",
                CustomerContactDetailsApiUrl = "https://this.is.anApi.org.uk/customers/{customerId}/ContactDetails/",
                CustomerContactDetailsApiVersion = "V2",
                ActionPlansApiUrl="SomeAPI",
                ActionPlansApiVersion = "V2",
                DigitalIdentitiesPatchByCustomerIdApiUrl = "https://at.api.nationalcareersservice.org.uk/digitalidentities/api/customer/{customerId}",
                DigitalIdentitiesPatchByCustomerIdApiVersion = "v2"
            });
        }

        [Test]
        public async Task When_UpdateLastLoginWithException_UpdateCustomer()
        {
            _dssService = new DssService(_restClient, _dssSettings, _logger);
            var token = "test";
            var result = await _dssService.UpdateLastLogin(new Guid(), token);
            result.Should().NotBe(null);
        }
        [Test]
        public async Task When_UpdateLastLoginWithException_Return_Exception()
        {
            var restClient = Substitute.For<IRestClient>();
            restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            var token = "test";

            restClient.GetAsync<string>(Arg.Any<string>(),Arg.Any<HttpRequestMessage>()).Returns<string>(x => { throw new DssException("Customer");});
            
            _dssService = new DssService(restClient, _dssSettings, _logger);

            _dssService.Invoking(async sut => await sut.UpdateLastLogin(new Guid(), token))
                .Should().Throw<DssException>();

        }
        
    }

}