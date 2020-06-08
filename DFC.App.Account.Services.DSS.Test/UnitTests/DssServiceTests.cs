using System;
using System.Collections;
using System.Collections.Generic;
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
using Castle.Core.Logging;
using DFC.App.Account.Services.DSS.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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
            var mockHandler = DssHelpers.GetMockMessageHandler("&$a", statusToReturn: HttpStatusCode.NoContent);
            _restClient = new RestClient(mockHandler.Object);
            _dssService = new DssService(_restClient, _dssSettings, _logger);

            _dssService.Invoking( sut=> sut.GetCustomerDetail("993cfb94-12b7-41c4-b32d-7be9331174f1",
                    new HttpRequestMessage()))
                .Should().Throw<DssException>()
                .WithMessage("Failure Customer Details*");


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

            _dssService.Invoking(x => x.DeleteCustomer(null)).Should()
                .Throw<UnableToUpdateCustomerDetailsException>();

        }

        [Test]
        public async Task When_DeleteCustomerSuccess_ReturnOk()
        {
            //Tod-Remove
            var deleteRequest = new DeleteCustomerRequest()
            {
                CustomerId = new Guid( "621b65a6-497e-4e9f-8031-a9a1a54a4b61"),
                ReasonForTermination = 1,
                DateOfTermination = DateTime.UtcNow
            };
           
            await _dssService.DeleteCustomer(deleteRequest);

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
            var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulDssCustomerActionPlanDetailsDetails(), statusToReturn: HttpStatusCode.Created);
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
            restClient.GetAsync<IList<ActionPlan>>(Arg.Any<string>(),Arg.Any<HttpRequestMessage>()).Returns<IList<ActionPlan>>(x => { throw new DssException("Failure Action Plans");});
            
            _dssService = new DssService(restClient, _dssSettings, _logger);

            _dssService.Invoking(sut => sut.GetActionPlans("993cfb94-12b7-41c4-b32d-7be9331174f1"))
                .Should().Throw<DssException>();

        }
    }
}