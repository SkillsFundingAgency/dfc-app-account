using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common;
using DFC.App.Account.Application.Common.Interfaces;
using DFC.App.Account.Services.Auth.Interfaces;
using DFC.App.Account.Services.Auth.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Services.Auth
{
    public class AzureB2CAuthClient : IOpenIDConnectClient
    {
        private readonly IRestClient _restClient;
        private readonly OpenIDConnectSettings _settings;

        public AzureB2CAuthClient(IOptions<OpenIDConnectSettings> settings)
        {
            _restClient = new RestClient();
            _settings = settings.Value;
        }

        public AzureB2CAuthClient(IOptions<OpenIDConnectSettings> settings, IRestClient client)
        {
            _restClient = client;
            _settings = settings.Value;
        }


        private string GetUrlWithoutParams(string url)
        {
            return new UriBuilder(url) { Query = string.Empty }.ToString();
        }

        public async Task<IResult> VerifyPassword(string userName, string password)
        {
            
            var tokenEndPoint = _settings.TokenEndpoint;
            var queryParams = new Dictionary<string, string>();
            queryParams.Add("p", "B2C_1_ROPC_Auth");
            queryParams.Add("grant_type", "password");
            queryParams.Add("client_id", _settings.PwdVerificationClientId);
            queryParams.Add("scope", $"openid {_settings.PwdVerificationClientId}");
            queryParams.Add("response_type", "token");
            queryParams.Add("username", userName );
            queryParams.Add("password", password);

            string verifyPasswordUrl = QueryHelpers.AddQueryString(GetUrlWithoutParams(_settings.TokenEndpoint), queryParams);

           
           var response = await _restClient.PostAsync<VerifyPasswordResponse>(verifyPasswordUrl, new StringContent(""));
           if (!String.IsNullOrEmpty(response?.AccessToken))
               return Result.Ok();

           return Result.Fail("Invalid Password");
        }
    }
}
