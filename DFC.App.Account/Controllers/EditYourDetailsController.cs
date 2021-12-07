using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Exceptions;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RedirectResult = Microsoft.AspNetCore.Mvc.RedirectResult;

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class EditYourDetailsController : CompositeSessionController<EditDetailsCompositeViewModel>
    {
        private const string ErrorMessageServiceUnavailable = "Find Address Service is currently unavailable. Please enter your address details in the boxes provided.";
        private readonly IDssReader _dssReader;
        private readonly IDssWriter _dssWriter;
        public const string SmsErrorMessage = "You have selected a contact preference which requires a valid mobile number";

        public EditYourDetailsController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,
            IDssReader dssReader, IDssWriter dssWriter, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, authService, documentService, config)
        {
            _dssReader = dssReader;
            _dssWriter = dssWriter;
        }

        [Route("/body/edit-your-details")]
        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            var customerDetails = await _dssReader.GetCustomerData(customer.CustomerId.ToString());
            ViewModel.Identity = MapCustomerToCitizenIdentity(customerDetails);

            return await base.Body();
        }

        [Route("/body/edit-your-details")]
        [HttpPost]
        public async Task<IActionResult> Body(EditDetailsCompositeViewModel viewModel, IFormCollection formCollection)
        {
            var customer = await GetCustomerDetails();
            var additionalData = GetEditDetailsAdditionalData(formCollection);
            viewModel.Identity.MarketingPreferences = new MarketingPreferences
            {
                OptOutOfMarketing = !additionalData.MarketingOptIn,
                OptOutOfMarketResearch = !additionalData.MarketResearchOptIn
            };
            
                var customerDetails = await _dssReader.GetCustomerData(customer.CustomerId.ToString());
                if (IsValid(ModelState, viewModel))
                {
                    try
                    {
                        
                        var dateOfBirthDay = viewModel.Identity.PersonalDetails.DateOfBirthDay;
                        var dateOfBirthMonth = viewModel.Identity.PersonalDetails.DateOfBirthMonth;
                        var dateOfBirthYear = viewModel.Identity.PersonalDetails.DateOfBirthYear;

                        CultureInfo enGb = new CultureInfo("en-GB");
                        string dob = string.Empty;
                        if (!string.IsNullOrEmpty(dateOfBirthDay) && !string.IsNullOrEmpty(dateOfBirthMonth) &&
                            !string.IsNullOrEmpty(dateOfBirthYear))
                        {
                            dob = $"{dateOfBirthDay.PadLeft(2, '0')}/{dateOfBirthMonth.PadLeft(2, '0')}/{dateOfBirthYear.PadLeft(4, '0')}";
                        }

                        if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal,
                            out var dateOfBirth))
                        {
                            viewModel.Identity.PersonalDetails.DateOfBirth = dateOfBirth;
                        }
                        else
                        {
                            viewModel.Identity.PersonalDetails.DateOfBirth = null;
                        }

                        var updatedDetails = GetUpdatedCustomerDetails(customerDetails, viewModel.Identity);
                        await _dssWriter.UpdateCustomerData(updatedDetails);
                        
                        updatedDetails.Contact.LastModifiedDate = DateTime.UtcNow.AddMinutes(-1);
                        
                        await _dssWriter.UpsertCustomerContactData(updatedDetails);

                        return new RedirectResult("/your-account/your-details", false);
                    }
                    catch (EmailAddressAlreadyExistsException)
                    {
                        ModelState.AddModelError("Identity.ContactDetails.ContactEmail", "Email address already in use");
                    }
                }
            
            ViewModel.Identity = viewModel.Identity;

            return await base.Body();
        }

        private static bool IsValid(ModelStateDictionary modelState, EditDetailsCompositeViewModel viewModel)
        {
            if (viewModel.Identity.ContactDetails.ContactPreference == CommonEnums.Channel.Text)
            {
                var contact = viewModel.Identity.ContactDetails;
                if (string.IsNullOrEmpty(contact.MobileNumber))
                {
                    modelState.AddModelError("Identity.ContactDetails.MobileNumber", SmsErrorMessage);
                    return false;
                }
            }

            return modelState.IsValid;
        }
        
        private static EditDetailsAdditionalData GetEditDetailsAdditionalData(IFormCollection formCollection)
        {
            return new EditDetailsAdditionalData
            {
                MarketResearchOptIn = !string.IsNullOrEmpty(formCollection.FirstOrDefault(x => string.Compare(x.Key, "marketResearchOptIn", StringComparison.CurrentCultureIgnoreCase) == 0).Value),
                MarketingOptIn = !string.IsNullOrEmpty(formCollection.FirstOrDefault(x => string.Compare(x.Key, "marketingOptIn", StringComparison.CurrentCultureIgnoreCase) == 0).Value),
            };

        }

        private Customer GetUpdatedCustomerDetails(Customer customer, CitizenIdentity identity)
        {
            customer.Contact.PreferredContactMethod = identity.ContactDetails.ContactPreference;
            customer.Contact.HomeNumber = identity.ContactDetails.HomeNumber ?? string.Empty;
            customer.Contact.MobileNumber = identity.ContactDetails.MobileNumber ?? string.Empty;
            customer.Contact.AlternativeNumber = identity.ContactDetails.TelephoneNumberAlternative ?? string.Empty;

            customer.OptInMarketResearch = identity.MarketingPreferences.MarketResearchOptIn;
            customer.OptInUserResearch = identity.MarketingPreferences.MarketingOptIn;

            customer.DateofBirth = identity.PersonalDetails.DateOfBirth;
            customer.FamilyName = identity.PersonalDetails.FamilyName;
            customer.Gender = identity.PersonalDetails.Gender;
            customer.GivenName = identity.PersonalDetails.GivenName;
            customer.Title = identity.PersonalDetails.Title;
            
            return customer;
        }

        private CitizenIdentity MapCustomerToCitizenIdentity(Customer customer)
        {
            return ViewModel.Identity = new CitizenIdentity
            {
                ContactDetails = new ContactDetails
                {
                    ContactEmail = customer.Contact?.EmailAddress,
                    ContactPreference = customer.Contact?.PreferredContactMethod ?? CommonEnums.Channel.Email,
                    HomeNumber = customer.Contact?.HomeNumber,
                    MobileNumber = customer.Contact?.MobileNumber,
                    TelephoneNumberAlternative = customer.Contact?.AlternativeNumber
                },
                MarketingPreferences = new MarketingPreferences
                {
                    OptOutOfMarketResearch = !customer.OptInMarketResearch,
                    OptOutOfMarketing = !customer.OptInUserResearch
                },
                PersonalDetails = new PersonalDetails
                {
                    DateOfBirth = customer.DateofBirth,
                    DateOfBirthDay = customer.DateofBirth?.Day.ToString(),
                    DateOfBirthMonth = customer.DateofBirth?.Month.ToString(),
                    DateOfBirthYear = customer.DateofBirth?.Year.ToString(),
                    FamilyName = customer.FamilyName,
                    Gender = customer.Gender,
                    GivenName = customer.GivenName,
                    Title = customer.Title
                }
            };
        }
    }
}