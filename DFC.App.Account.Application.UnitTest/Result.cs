using NUnit.Framework;
using DFC.App.Account.Application.Common;

namespace DFC.App.Account.Application.UnitTest
{
    public class Tests
    {
        
        [Test]
        public void When_Result_Ok_ReturnOkResult()
        {
            var ok =   Result.Ok();
            var error = Result.Fail("Something failed");
        }
    }
}