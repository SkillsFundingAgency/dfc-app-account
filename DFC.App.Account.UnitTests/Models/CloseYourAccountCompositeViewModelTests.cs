using DFC.App.Account.ViewModels;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Models
{
    class CloseYourAccountCompositeViewModelTests
    {
        [Test]
        public void CloseYourAccountCompositeViewModel()
        {
            var closeYourAccountCompositeViewModel = new CloseYourAccountCompositeViewModel();
            closeYourAccountCompositeViewModel.Password = "Password";
            closeYourAccountCompositeViewModel.Reason = "Reason";
        }
    }
}