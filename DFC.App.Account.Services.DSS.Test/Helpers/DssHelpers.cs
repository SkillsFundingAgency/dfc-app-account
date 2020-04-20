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
