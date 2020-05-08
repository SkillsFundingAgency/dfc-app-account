using DFC.App.Account.Models;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Models
{
    class DeleteAccountRequestTests
    {
        [Test]
        public void CreateDeleteAccountRequest()
        {
            var deleteAccountRequest = new DeleteAccountRequest();
            deleteAccountRequest.Password = "password";
            deleteAccountRequest.Reason = "some reason";
        }
    }
}
