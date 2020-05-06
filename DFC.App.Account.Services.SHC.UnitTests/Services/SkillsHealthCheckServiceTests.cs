using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Services;
using DFC.App.Account.Services.SHC.Models;
using DFC.App.Account.Services.SHC.Services;
using DFC.App.Account.Services.SHC.UnitTest.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.Account.Services.SHC.UnitTest.Services
{
    public class SkillsHealthCheckServiceTests
    {
        [Test]
        public async Task IfIdIsNull_ReturnNull()
        {
            var factory = new HttpWebRequestFactory();
            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory);
            var documents = serviceUnderTest.GetSHCDocumentsForUser(null);
            documents.Should().BeNull();
        }

        [Test]
        public async Task IfSuccessfulCall_ReturnDocuments()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.SuccessfulCall(), HttpStatusCode.OK);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory);
            var documents = serviceUnderTest.GetSHCDocumentsForUser("12345");
            documents.Should().NotBeNull();
            documents.Should().NotBeEmpty();
            documents.Count.Should().Be(2);
        }
        [Test]
        public async Task IfSuccessfulCall_ButNoData_ReturnEmptyList()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.EmptyResult(), HttpStatusCode.OK);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory);
            var documents = serviceUnderTest.GetSHCDocumentsForUser("12345");
            documents.Should().NotBeNull();
            documents.Should().BeEmpty();
            documents.Count.Should().Be(0);
        }
        [Test]
        public async Task IfUnsuccessfulCall_ReturnNull()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.EmptyResult(), HttpStatusCode.InternalServerError);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory);
            var documents = serviceUnderTest.GetSHCDocumentsForUser("12345");
            documents.Should().BeNull();
        }

    }
}
