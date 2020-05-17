using DFC.App.Account.Services.Auth.Models;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common;
using DFC.App.Account.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace DFC.App.Account.Services.AzureB2CAuth
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

        private string GetUrlWithoutParams(string url)
        {
            return new UriBuilder(url) { Query = string.Empty }.ToString();
        }

        private async Task LoadOpenIDConnectConfig()
        {
            OpenIDConnectConfig config = await _restClient.GetAsync<OpenIDConnectConfig>(_settings.OIDCConfigMetaDataUrl);
            _settings.AuthorizeUrl = GetUrlWithoutParams(config.AuthorizationEndpoint);
            _settings.JWKsUrl = config.JwksUri;
            _settings.Issuer = config.Issuer;
            _settings.TokenEndpoint = config.TokenEndpoint;
        }

        private async Task LoadJsonWebKeyAsync()
        {
            JObject response = await _restClient.GetAsync<JObject>(_settings.JWKsUrl);
            var keys = response["keys"];
            var keyArray = keys.ToObject<JsonWebKey[]>();
            _settings.JWK = keyArray[0].N;
        }

        private byte[] FromBase64Url(string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                                  .Replace("-", "+");
            return Convert.FromBase64String(base64);
        }

        private RsaSecurityKey GetRsaSecurityKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
              new RSAParameters()
              {
                  Modulus = FromBase64Url(_settings.JWK),
                  Exponent = FromBase64Url("AQAB")
              });
            var rsaKey = new RsaSecurityKey(rsa);
            
            return rsaKey;
        }

        public async Task<string> GetRegisterUrl()
        {
            await LoadSettings();

            var queryParams = new Dictionary<string, string>();
            queryParams.Add("p", "B2C_1A_account_signup");
            queryParams.Add("client_id", _settings.ClientId);
            queryParams.Add("nonce", "defaultNonce");
            queryParams.Add("redirect_uri", _settings.RedirectUrl);
            queryParams.Add("scope", "openid");
            queryParams.Add("response_type", "id_token");
            queryParams.Add("prompt", "login");
            
            string registerUrl = QueryHelpers.AddQueryString(_settings.AuthorizeUrl, queryParams);
            
            return registerUrl;
        }

        public async Task<string> GetSignInUrl()
        {
            if (_settings.UseOIDCConfigDiscovery)
            {
                await LoadOpenIDConnectConfig();
            }

            var queryParams = new Dictionary<string, string>();
            queryParams.Add("p", "B2C_1A_signin_invitation");
            queryParams.Add("client_id", _settings.ClientId);
            queryParams.Add("nonce", "defaultNonce");
            queryParams.Add("redirect_uri", _settings.RedirectUrl);
            queryParams.Add("scope", "openid");
            queryParams.Add("response_type", "id_token");
            queryParams.Add("response_mode", "query");
            queryParams.Add("prompt", "login");
            string registerUrl = QueryHelpers.AddQueryString(_settings.AuthorizeUrl, queryParams);

            return registerUrl;
        }

       
        public async Task<JwtSecurityToken> ValidateToken(string token)
        {
            if (_settings.UseOIDCConfigDiscovery)
            {
                await LoadOpenIDConnectConfig();
                await LoadJsonWebKeyAsync();
            }

            // Do we include Personally Identifiable Information in any exceptions or logging?
            IdentityModelEventSource.ShowPII = _settings.LogPersonalInfo;
        
            var tokenHandler = new JwtSecurityTokenHandler();
            // This will throw an exception if the token fails to validate.
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetRsaSecurityKey(),
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            return validatedToken as JwtSecurityToken;
        }

        public async Task<IResult> VerifyPassword(string userName, string password)
        {
            await LoadSettings();

            var queryParams = new Dictionary<string, string>();
            queryParams.Add("p", "B2C_1_ROPC_Auth");
            queryParams.Add("grant_type", "password");
            queryParams.Add("client_id", _settings.ClientId);
            queryParams.Add("scope", $"openid {_settings.ClientId}");
            queryParams.Add("response_type", "token");
            queryParams.Add("username", userName );
            queryParams.Add("password", password);

            string verifyPasswordUrl = QueryHelpers.AddQueryString(GetUrlWithoutParams(_settings.TokenEndpoint), queryParams);

           
           var response = await _restClient.PostAsync<VerifyPasswordResponse>(verifyPasswordUrl,
               new StringContent("")) as VerifyPasswordResponse;
           if (!String.IsNullOrEmpty(response?.AccessToken))
               return Result.Ok();

           return Result.Fail("Invalid Password");
        }

        public string GetAuthdUrl()
        {
            return _settings.AuthdUrl;
        }


        private async Task LoadSettings()
        {
            if(_settings.UseOIDCConfigDiscovery)
            {
                await LoadOpenIDConnectConfig();
            }
        }

    }
}
