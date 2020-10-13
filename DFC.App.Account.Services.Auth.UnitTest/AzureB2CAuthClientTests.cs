using DFC.App.Account.Services.Auth.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common;
using DFC.Personalisation.Common.Net.RestClient;
using NSubstitute;

namespace DFC.App.Account.Services.Auth.UnitTest
{
    public class AzureB2CAuthClientTests
    {
        public class VerifyPassword
        {
            [Test]
            public async Task When_InCorrectPasswordProvided_Return_Failure()
            {
                var client = Substitute.For<IRestClient>();
                client.PostAsync<VerifyPasswordResponse>(Arg.Any<string>(), Arg.Any<StringContent>()).ReturnsForAnyArgs(new VerifyPasswordResponse());
                // Arrange
                var authClient = new AzureB2CAuthClient(Options.Create(new OpenIDConnectSettings()
                {
                    TokenEndpoint = "http://www.something.com",
                    PwdVerificationClientId = "some id"
                }), client);

                // Act
                var result = await authClient.VerifyPassword("something","password");

                // Assert
                result.Should().Be(Result.Fail("Invalid Password"));
            }

            [Test]
            public async Task When_CorrectPasswordProvided_Return_Success()
            {
                var client = Substitute.For<IRestClient>();
                client.PostAsync<VerifyPasswordResponse>(Arg.Any<string>(), Arg.Any<StringContent>()).ReturnsForAnyArgs(new VerifyPasswordResponse{
                AccessToken = "test"});
                // Arrange
                var authClient = new AzureB2CAuthClient(Options.Create(new OpenIDConnectSettings()
                {
                    TokenEndpoint = "http://www.something.com",
                    PwdVerificationClientId = "some id"
                }), client);

                // Act
                var result = await authClient.VerifyPassword("something", "password");

                // Assert
                result.Should().Be(Result.Ok());
            }
        }
    }
}