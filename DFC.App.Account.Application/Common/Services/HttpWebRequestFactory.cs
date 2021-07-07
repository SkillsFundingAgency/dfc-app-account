using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace DFC.App.Account.Application.Common.Services
{
    public interface IHttpWebRequest
    {

        WebHeaderCollection Headers { get; set; }
        string Accept { get; set; }
        string ContentType { get; set; }
        string Method { get; set; }
        Stream GetRequestStream();
        IHttpWebResponse GetResponse();
    }

    public interface IHttpWebResponse : IDisposable
    {
        HttpStatusCode StatusCode { get; set; }
        // expose the members you need
        Stream GetResponseStream();

    }

    public interface IHttpWebRequestFactory
    {
        IHttpWebRequest Create(string uri);
    }

    [ExcludeFromCodeCoverage]
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        public IHttpWebRequest Create(string uri)
        {
            return new WrapHttpWebRequest((HttpWebRequest)WebRequest.Create(uri));
        }
    }

    [ExcludeFromCodeCoverage]
    public class WrapHttpWebRequest : IHttpWebRequest
    {
        private readonly HttpWebRequest _request;

        public WrapHttpWebRequest(HttpWebRequest request)
        {
            _request = request;
        }

        public WebHeaderCollection Headers
        {         
            get => _request.Headers;
            set => _request.Headers = value;
        }
    
        public string Accept
        {
            get => _request.Accept;
            set => _request.Accept = value;
        }
        public string ContentType
        {
            get => _request.ContentType;
            set => _request.ContentType = value;
        }

        public string Method
        {
            get => _request.Method;
            set => _request.Method = value;
            
        }

        public Stream GetRequestStream()
        {
            return _request.GetRequestStream();
        }

        public IHttpWebResponse GetResponse()
        {
            return new WrapHttpWebResponse((HttpWebResponse)_request.GetResponse());
        }
    }

    [ExcludeFromCodeCoverage]
    public class WrapHttpWebResponse : IHttpWebResponse
    {
        private WebResponse _response;

        public WrapHttpWebResponse(HttpWebResponse response)
        {
            _response = response;
            StatusCode = response.StatusCode;
            
        }

        public HttpStatusCode StatusCode { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_response != null)
                {
                    ((IDisposable)_response).Dispose();
                    _response = null;
                }
            }
        }

        public Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }
    }
}
