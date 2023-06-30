using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Application.Common.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.Services.SHC.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Xml;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.Services.SHC.Services
{
    public class SkillsHealthCheckService : ISkillsHealthCheckService
    {
        private readonly IOptions<ShcSettings> _settings;
        private readonly ILogger _logger;
        private readonly IHttpWebRequestFactory _factory;
        private const string ContentType = "text/xml;charset=\"utf-8\"";
        public SkillsHealthCheckService(IOptions<ShcSettings> settings, IHttpWebRequestFactory requestFactory, ILogger<SkillsHealthCheckService> logger)
        {
            
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrEmpty(settings.Value.Url, nameof(settings.Value.Url));
            Throw.IfNullOrEmpty(settings.Value.FindDocumentsAction, nameof(settings.Value.FindDocumentsAction));
            Throw.IfNullOrEmpty(settings.Value.SHCDocType, nameof(settings.Value.SHCDocType));
            Throw.IfNullOrEmpty(settings.Value.ServiceName, nameof(settings.Value.ServiceName));
            Throw.IfNullOrEmpty(settings.Value.LinkUrl, nameof(settings.Value.LinkUrl));

            Throw.IfNull(requestFactory, nameof(requestFactory));

            _settings = settings;
            _factory = requestFactory;
            _logger = logger;
        }
        public List<ShcDocument> GetShcDocumentsForUser(string llaId)
        {
            if (string.IsNullOrWhiteSpace(llaId))
            {
                _logger.LogWarning($"SkillsHealthCheckService GetShcDocumentsForUser llaId is null");
                return new List<ShcDocument>();
            }
            

            var soapEnvelopeXml = CreateSoapEnvelope(llaId);
            var webRequest = CreateWebRequest(_settings.Value.Url, _settings.Value.FindDocumentsAction);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            var result = webRequest.GetResponse();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"Failure to get SHC document. LLA ID: {llaId}");
                throw new ShcException($"Failure to get SHC document. LLA ID: {llaId}, Code: {result.StatusCode}");
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
            webRequest.ContentType = ContentType;
            webRequest.Accept = MediaTypeNames.Text.Xml;
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

        private List<ShcDocument> ParseShcDocuments(ShcDocumentResponse response)
        {
            var documents =  response.Body.FindDocumentsByServiceNameKeyValueAndDocTypeResponse
                .FindDocumentsByServiceNameKeyValueAndDocTypeResult.SkillsDocument;
            if (documents.Any())
            {
                var shcDocs = Mapping.Mapper.Map<List<ShcDocument>>(documents);
                return GenerateLinkUrls(shcDocs);
            }
            return new List<ShcDocument>();
        }

        private  List<ShcDocument> GenerateLinkUrls(List<ShcDocument> documents)
        {
            foreach (var doc in documents)
            {
                doc.LinkUrl = $"{_settings.Value.LinkUrl}{doc.DocumentId}";
            }

            return documents;
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
