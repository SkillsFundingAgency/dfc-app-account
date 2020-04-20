using FluentAssertions;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Integration
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
                            .AddJsonFile("appsettings.json")
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
            var result = await _client.GetAsync("/home/index");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
