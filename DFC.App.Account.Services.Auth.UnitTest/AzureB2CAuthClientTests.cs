using DFC.App.Account.Services.AzureB2CAuth;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.Account.Services.Auth.UnitTest
{
    public class AzureB2CAuthClientTests
    {
        /*
        [Test]
        public async Task TDD1()
        {
            var authClient = new AzureB2CAuthClient(Options.Create(new Models.OpenIDConnectSettings() 
            { 
                OIDCConfigMetaDataUrl = "https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1A_signin_invitation", 
                ClientId = "" 
            }));

            var config = await authClient.GetConfigAsync();

            config.Should().NotBeNull();
            config.AuthorizationEndpoint.Should().NotBeNullOrEmpty();
            config.EndSessionEndpoint.Should().NotBeNullOrEmpty();
            config.Issuer.Should().NotBeNullOrEmpty();
            config.JwksUri.Should().NotBeNullOrEmpty();
            config.TokenEndpoint.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task TDD2()
        {
            var authClient = new AzureB2CAuthClient(Options.Create(new Models.OpenIDConnectSettings()
            {
                OIDCConfigMetaDataUrl = "https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1A_signin_invitation",
                ClientId = ""
            }));

            var key = await authClient.GetJsonWebKeyAsync("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/discovery/v2.0/keys?p=b2c_1a_signin_invitation");

            key.Should().NotBeNull();
        }

        [Test]
        public void TDD3()
        {
            var authClient = new AzureB2CAuthClient(Options.Create(new Models.OpenIDConnectSettings()
            {
                OIDCConfigMetaDataUrl = "https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1A_signin_invitation",
                ClientId = ""
            }));

            var url = authClient.GetRegisterUrl("https://authorizeUrl", "https://redirect");

            url.Should().Be("https://authorizeUrl?p=B2C_1A_account_signup&client_id=myclientid&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fredirect&scope=openid&response_type=id_token&prompt=login");
        }
        */
    }
}