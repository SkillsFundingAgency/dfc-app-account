using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Account.Services.DSS.UnitTests.Helpers
{
    public class DssHelpers
    {
        public static string SuccessfulDssCustomerCreation()
        {
            #region Result
            return
                "{  \"CustomerId\": \"59b2b839-cd14-44db-80d4-4d383795a9ef\",  \"DateOfRegistration\": \"2018-06-21T14:45:00\",  \"Title\": 1,  \"GivenName\": \"Boris\",  \"FamilyName\": \"Johnson\",  \"DateofBirth\": \"2000-06-21T14:45:00\",  \"Gender\": 3,  \"UniqueLearnerNumber\": \"3000000000\",  \"OptInUserResearch\": true,  \"OptInMarketResearch\": true,  \"DateOfTermination\": \"2000-06-21T14:45:00\",  \"ReasonForTermination\": 3,  \"IntroducedBy\": 1,  \"IntroducedByAdditionalInfo\": \"Customer was introduced to NCS by party X on date Y\",  \"SubcontractorId\": \"\",  \"LastModifiedDate\": \"2018-06-21T14:45:00\",  \"LastModifiedTouchpointId\": \"9000000000\"}";

            #endregion
        }

        public static string SuccessfulDssCustomerContactDetails()
        {
            return
                "{ \"ContactId\": \"e60063c7-d0e9-424c-9b41-f3708c1977ae\",\n            \"CustomerId\": \"edd0c701-71dd-48e7-928a-cc3b80090d86\",\n            \"PreferredContactMethod\": 1,\n            \"MobileNumber\": \"07676123457\",\n            \"HomeNumber\": \"08654123456\",\n            \"AlternativeNumber\": \"08654123458\",\n            \"EmailAddress\": \"billy@sausage.com\",\n            \"LastModifiedDate\": \"2018-11-16T09:01:48.2522789Z\",\n            \"LastModifiedTouchpointId\": \"9000000000\"}";
        }

        public static string SuccessfulDssCustomerAddressDetails()
        {
            return
                "[{  \"AddressId\": \"fff9e1c9-4c3d-45db-ae1d-b46ad5e8e6c0\",\n   \"Address1\": \"Test Apartment 45\",\n    \"Address2\": \"blah\",\n    \"Address3\": \"Adddress Line 3\",\n    \"Address4\": \"Adddress Line 4\",\n    \"Address5\": \"Adddress Line 5\",\n    \"PostCode\": \"AB12CD\",\n    \"AlternativePostCode\": \"DE15 0EU\",\n    \"Longitude\": -2.08925,\n    \"Latitude\": 57.11622,\n    \"EffectiveFrom\": \"2018-06-19T09:01:00Z\",\n    \"EffectiveTo\": null,\n    \"LastModifiedDate\": \"2019-11-19T08:57:37.8205974Z\",\n    \"LastModifiedTouchpointId\": \"0000000000\",\n    \"SubcontractorId\": \"21323234\"}]";
        }
        
        public static string SuccessfulDssCustomerActionPlanDetails()
        {
            return
                "[{\r\n  \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"InteractionId\": \"2817ea6b-a1d6-4e1a-8eba-46b7d1a427ac\",\r\n  \"SessionId\": \"e3ebf979-4484-4f18-a08f-5f8a4fdde35a\",\r\n  \"SubcontractorId\": \"\",\r\n  \"DateActionPlanCreated\": \"2020-05-10T08:30:00+00:00\",\r\n  \"CustomerCharterShownToCustomer\": true,\r\n  \"DateAndTimeCharterShown\": \"2020-05-10T08:30:00+00:00\",\r\n  \"DateActionPlanSentToCustomer\": \"2020-05-10T08:30:00+00:00\",\r\n  \"ActionPlanDeliveryMethod\": 2,\r\n  \"DateActionPlanAcknowledged\": \"2020-05-10T08:30:00+00:00\",\r\n  \"CurrentSituation\": \"this is some text 1st ActionPlan\",\r\n  \"LastModifiedDate\": \"2020-05-10T13:45:00\",\r\n  \"LastModifiedTouchpointId\": \"9000000000\"\r\n}]";
        }
        
        public static string SuccessfulDigitalIdentitiesCustomerUpdateDetails()
        {
            return
                "{\r\n  \"IdentityID\": \"2aa694e4-190b-4911-83a6-cb6f7f54bd63\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"IdentityStoreId\": \"de1b2c64-77ee-47b2-a313-81659c7ae85c\",\r\n  \"LegacyIdentity\": null,\r\n  \"id_token\": null,\r\n  \"LastLoggedInDateTime\": \"2020-06-16T13:45:00\",\r\n  \"LastModifiedDate\": \"2020-07-16T13:41:12.8766884Z\",\r\n  \"LastModifiedTouchpointId\": \"0000000997\",\r\n  \"DateOfTermination\": null\r\n}";
        }
        public static Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn = "{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn)
                })
                .Verifiable();
            return handlerMock;
        }

    }
}
