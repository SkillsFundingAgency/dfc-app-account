using DFC.App.Account.Services.Auth.Models;
using DFC.App.Account.Services.AzureB2CAuth;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.Account.Services.Auth.UnitTest
{
    public class AzureB2CAuthClientTests
    {
        public class GetRegisterUrl
        {
            [Test]
            public async Task When_ConfiguredWithoutDiscovery_Then_ReturnValidUrlWithParams()
            {
                // Arrange
                var authClient = new AzureB2CAuthClient(Options.Create(new OpenIDConnectSettings()
                {
                    LogPersonalInfo = false,
                    UseOIDCConfigDiscovery = false,
                    AuthorizeUrl = "https://authorize",
                    ClientId = "thisismyclientid",
                    Issuer = "",
                    JWK = "",
                    RedirectUrl = "http://redirect-to/this-page",
                }));

                // Act
                var url = await authClient.GetRegisterUrl();

                // Assert
                url.Should().Be("https://authorize?p=B2C_1A_account_signup&client_id=thisismyclientid&nonce=defaultNonce&redirect_uri=http%3A%2F%2Fredirect-to%2Fthis-page&scope=openid&response_type=id_token&prompt=login");
            }
        }

        public class GetSignInUrl
        {
            [Test]
            public async Task When_ConfiguredWithoutDiscovery_Then_ReturnValidUrlWithParams()
            {
                // Arrange
                var authClient = new AzureB2CAuthClient(Options.Create(new OpenIDConnectSettings()
                {
                    LogPersonalInfo = false,
                    UseOIDCConfigDiscovery = false,
                    AuthorizeUrl = "https://authorize",
                    ClientId = "thisismyclientid",
                    Issuer = "",
                    JWK = "",
                    RedirectUrl = "http://redirect-to/this-page",
                }));

                // Act
                var url = await authClient.GetSignInUrl();

                // Assert
                url.Should().Be("https://authorize?p=B2C_1A_signin_invitation&client_id=thisismyclientid&nonce=defaultNonce&redirect_uri=http%3A%2F%2Fredirect-to%2Fthis-page&scope=openid&response_type=id_token&response_mode=query&prompt=login");
            }
        }

        public class ValidateToken
        {
            [Test]
            public void When_ConfiguredWithoutDiscovery_And_TokenValid_Then_ReturnValidatedToken()
            {
            }

            [Test]
            public async Task When_ConfiguredWithoutDiscovery_And_TokenInvalid_Then_ThrowException()
            {
                // Arrange
                var authClient = new AzureB2CAuthClient(Options.Create(new OpenIDConnectSettings()
                {
                    LogPersonalInfo = false,
                    UseOIDCConfigDiscovery = false,
                    AuthorizeUrl = "https://authorize",
                    ClientId = "thisismyclientid",
                    Issuer = "",
                    JWK = "",
                    RedirectUrl = "http://redirect-to/this-page",
                }));

                // Act
                Func<Task> act = async () => { await authClient.ValidateToken("somenonsensetokenthisisnevergoingtovalidate"); };

                // Assert
                await act.Should().ThrowAsync<Exception>();
            }
        }
    }
}