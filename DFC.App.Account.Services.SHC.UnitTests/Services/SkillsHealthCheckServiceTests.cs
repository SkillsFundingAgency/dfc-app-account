using DFC.App.Account.Application.Common.Services;
using DFC.App.Account.Services.SHC.Models;
using DFC.App.Account.Services.SHC.Services;
using DFC.App.Account.Services.SHC.UnitTest.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Net;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DFC.App.Account.Services.SHC.UnitTest.Services
{
    public class SkillsHealthCheckServiceTests
    {
        private ILogger _logger;

        [Test]
        public void IfIdIsNull_ReturnNull()
        {
            var factory = new HttpWebRequestFactory();
            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName",
                LinkUrl = "linkUrl"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory, Substitute.For<ILogger>());
            var documents = serviceUnderTest.GetShcDocumentsForUser(null);
            documents.Should().BeEmpty();
        }

        [Test]
        public void IfSuccessfulCall_ReturnDocuments()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.SuccessfulCall(), HttpStatusCode.OK);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName",
                LinkUrl = "linkUrl"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory, Substitute.For<ILogger>());
            var documents = serviceUnderTest.GetShcDocumentsForUser("12345");
            documents.Should().NotBeNull();
            documents.Should().NotBeEmpty();
            documents.Count.Should().Be(2);
        }
        [Test]
        public void IfSuccessfulCall_ButNoData_ReturnEmptyList()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.EmptyResult(), HttpStatusCode.OK);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName",
                LinkUrl = "linkUrl"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory, Substitute.For<ILogger>());
            var documents = serviceUnderTest.GetShcDocumentsForUser("12345");
            documents.Should().NotBeNull();
            documents.Should().BeEmpty();
            documents.Count.Should().Be(0);
        }
        [Test]
        public void IfUnsuccessfulCall_ReturnNull()
        {
            var factory = new MockHttpWebFactory().CreateMockFactory(HttpWebRequestHelper.EmptyResult(), HttpStatusCode.InternalServerError);

            var settings = new ShcSettings
            {
                Url = "url",
                SHCDocType = "docType",
                FindDocumentsAction = "findDocumentsAction",
                ServiceName = "serviceName",
                LinkUrl = "linkUrl"
            };
            var serviceUnderTest = new SkillsHealthCheckService(Options.Create(settings), factory, Substitute.For<ILogger>());
            serviceUnderTest.Invoking(x => x.GetShcDocumentsForUser("12345")).Should().Throw<ShcException>()
                .WithMessage("Failure to get SHC document. LLA ID: 12345, Code: InternalServerError");

        }

    }
}
