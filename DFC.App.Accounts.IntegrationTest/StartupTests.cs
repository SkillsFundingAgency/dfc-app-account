using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using System.Net.Http;

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

    }
}
