using System.Collections.Generic;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.CustomAttributes;
using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using NSubstitute;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class EditDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;


        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new EditDetailsController(_compositeSettings, _authService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void AssigningViewModelProperties()
        {
            var editViewModel = new EditDetailsCompositeViewModel();
            editViewModel.Identity = new CitizenIdentity
            {
                ContactDetails = new ContactDetails
                {
                    ContactEmail = "ContactEmail",
                    ContactPreference = CommonEnums.Channel.Email,
                    TelephoneNumber = "8675309",
                    TelephoneNumberAlternative = "2222"
                },
                MarketingPreferences = new MarketingPreferences
                {
                    MarketResearchOptIn = true,
                    MarketingOptIn = true,
                    OptOutOfMarketResearch = false,
                    OptOutOfMarketing = false
                },
                PersonalDetails = new PersonalDetails
                {
                    AddressLine1 = "Line1",
                    AddressLine2 = "Line2",
                    AddressLine3 = "Line3",
                    AddressLine4 = "Line4",
                    AddressLine5 = "Line5",
                    AlternativePostCode = "SSSS",
                    DateOfBirth = null,
                    DateOfBirthDay = "05",
                    DateOfBirthMonth = "Oct",
                    DateOfBirthYear = "1990",
                }
            };
            editViewModel.Items = new List<PostalAddressModel>
            {
                new PostalAddressModel
                {
                    Cause = "Cause",
                    City = "City",
                    Description = "Description",
                    Error = 0,
                    Id = "Id",
                    Line1 = "Line1"
                }
            };
            var identity = editViewModel.Identity;
            var items = editViewModel.Items;

            editViewModel.Should().NotBeNull();
            identity.Should().NotBeNull();
            items.Should().NotBeNull();
        }

        [Test]
        public void AssigningValuesToPostalAddressViewModel()
        {
            var postalModel = new PostalAddressViewModel();
            postalModel.Items = new List<PostalAddressModel>
            {
                new PostalAddressModel
                {
                    Cause = "Cause",
                    City = "City",
                    Description = "Description",
                    Error = 0,
                    Id = "Id",
                    Line1 = "Line1"
                }
            };
            postalModel.SelectedItem = new PostalAddressModel
            {
                Cause = "Cause",
                City = "City",
                Description = "Description",
                Error = 0,
                Id = "Id",
                Line1 = "Line1"
            };

            var items = postalModel.Items;
            var selectedItems = postalModel.SelectedItem;

            postalModel.Should().NotBeNull();
            items.Should().NotBeNull();
            selectedItems.Should().NotBeNull();
        }
    }
}
