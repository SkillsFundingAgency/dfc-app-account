using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class EditDetailsController : CompositeSessionController<EditDetailsCompositeViewModel>
    {
        public EditDetailsController(IOptions<CompositeSettings> compositeSettings, IAuthService authService)
            : base(compositeSettings, authService)
        {
            
        }
        public override async Task<IActionResult> Body()
        {
            ViewModel.Identity = new CitizenIdentity
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
            ViewModel.Items = new List<PostalAddressModel>
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
            return await base.Body();
        }
    }
}