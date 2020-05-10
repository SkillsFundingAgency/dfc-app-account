using System.IO;
using System.Net;
using System.Text;
using DFC.App.Account.Application.Common.Services;
using NSubstitute;

namespace DFC.App.Account.Services.SHC.UnitTest.Helpers
{
    public class MockHttpWebFactory
    {
        public IHttpWebRequestFactory CreateMockFactory(string contentToReturn, HttpStatusCode statusCode)
        {
            // arrange
            var expected = contentToReturn;
            var expectedBytes = Encoding.UTF8.GetBytes(expected);

            var requestStreamText =
                "<soapenv:Envelope xmlns:soapenv=\\\"http://schemas.xmlsoap.org/soap/envelope/\\\" xmlns:ns=\\\"http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0\\\">  <soapenv:Header/>  <soapenv:Body>     <ns:FindDocumentsByServiceNameKeyValueAndDocType>        <ns:docType></ns:docType>        <ns:serviceName></ns:serviceName>        <ns:id></ns:id>        <ns:incData></ns:incData>     </ns:FindDocumentsByServiceNameKeyValueAndDocType>  </soapenv:Body></soapenv:Envelope>";
            var requestStreamBytes = Encoding.UTF8.GetBytes(requestStreamText);
            var requestStream = new MemoryStream();
            requestStream.Write(requestStreamBytes, 0, requestStreamBytes.Length);
            requestStream.Seek(0, SeekOrigin.Begin);

            
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = Substitute.For<IHttpWebResponse>();
            response.GetResponseStream().Returns(responseStream);
            response.StatusCode.Returns(statusCode);


            var request = Substitute.For<IHttpWebRequest>();
            request.GetRequestStream().Returns(requestStream);
            request.GetResponse().Returns(response);

            var factory = Substitute.For<IHttpWebRequestFactory>();
            factory.Create(Arg.Any<string>()).Returns(request);
            return factory;
        }
    }
}
