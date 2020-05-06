using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.Services.SHC.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using DFC.App.Account.Application.Common.Services;

namespace DFC.App.Account.Services.SHC.Services
{
    public class SkillsHealthCheckService : ISkillsHealthCheckService
    {
        private readonly IOptions<ShcSettings> _settings;
        private readonly IHttpWebRequestFactory _factory;
        public SkillsHealthCheckService(IOptions<ShcSettings> settings, IHttpWebRequestFactory requestFactory)
        {
            
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrEmpty(settings.Value.Url, nameof(settings.Value.Url));
            Throw.IfNullOrEmpty(settings.Value.FindDocumentsAction, nameof(settings.Value.FindDocumentsAction));
            Throw.IfNullOrEmpty(settings.Value.SHCDocType, nameof(settings.Value.SHCDocType));
            Throw.IfNullOrEmpty(settings.Value.ServiceName, nameof(settings.Value.ServiceName));

            Throw.IfNull(requestFactory, nameof(requestFactory));

            _settings = settings;
            _factory = requestFactory;
        }
        public List<SkillsDocument> GetSHCDocumentsForUser(string llaId)
        {
            if (string.IsNullOrWhiteSpace(llaId))
                return null;
            

            var soapEnvelopeXml = CreateSoapEnvelope(llaId);
            var webRequest = CreateWebRequest(_settings.Value.Url, _settings.Value.FindDocumentsAction);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            var result = webRequest.GetResponse();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var rd = new StreamReader(result.GetResponseStream() ?? throw new InvalidOperationException());
            var soapResult = rd.ReadToEnd();
            var deserialisedResponse = Deserialize<ShcDocumentResponse>(soapResult);
            return ParseShcDocuments(deserialisedResponse);
            
        }

        private IHttpWebRequest CreateWebRequest(string url, string action)
        {
            var webRequest = _factory.Create(url);
            webRequest.Headers = new WebHeaderCollection();
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope(string llaId)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0"">
               <soapenv:Header/>
               <soapenv:Body>
                  <ns:FindDocumentsByServiceNameKeyValueAndDocType>
                     <ns:docType>{_settings.Value.SHCDocType}</ns:docType>
                     <ns:serviceName>{_settings.Value.ServiceName}</ns:serviceName>
                     <ns:id>{llaId}</ns:id>
                     <ns:incData>false</ns:incData>
                  </ns:FindDocumentsByServiceNameKeyValueAndDocType>
               </soapenv:Body>
            </soapenv:Envelope>");
            return soapEnvelopeDocument;
        }

        private List<SkillsDocument> ParseShcDocuments(ShcDocumentResponse response)
        {
            return response.Body.FindDocumentsByServiceNameKeyValueAndDocTypeResponse
                .FindDocumentsByServiceNameKeyValueAndDocTypeResult.SkillsDocument;
        }
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, IHttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        private T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }



    }
}
