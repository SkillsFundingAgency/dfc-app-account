using DFC.App.Account.Exception;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Exception
{
    public class ExceptionTests
    {
        [Test]
        public void WhenNoUserIdInClaimExceptionISthrownMessageIsCorrect()
        {
            var exception = new NoUserIdInClaimException("Test");

            exception.Message.Should().Be("Test");
        }

        [Test]
        public void When_UserIsNotValidated_UserNotValidatedException_Is_ThrownWithCorrectMessage()
        {
            var exception = new UserNotValidatedException("Test");

            exception.Message.Should().Be("Test");
        }
    }
}
