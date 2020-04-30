using DFC.App.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.Accounts.IntegrationTest
{
    [TestFixture]
    public class StartupTests
    {
        private TestServer _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            var projectDir = Directory.GetCurrentDirectory();
            _factory = new TestServer(
                new WebHostBuilder()
                    .UseStartup<Startup>()
                    .UseConfiguration(
                        (new ConfigurationBuilder()
                            .SetBasePath(projectDir)
                            .AddJsonFile("appsettings-template.json")
                            .Build()
                        )));
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
        [Test]
        public async Task Tdd()
        {
            var result = await _client.GetAsync("/body");
            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }
    }
}
