using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.APP.Account.Data.Contracts
{
    public interface IWebhookContentProcessor
    {
        Task<HttpStatusCode> ProcessContentAsync(Uri url, Guid contentId);

        Task<HttpStatusCode> DeleteContentAsync(Guid contentId);


    }
}
