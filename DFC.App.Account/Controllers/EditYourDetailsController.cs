using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Exceptions;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.Interfaces;
using DFC.App.Account.ViewModels;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private readonly IAddressSearchService _addressSearchService;
        private readonly IDssReader _dssReader;
        private readonly IDssWriter _dssWriter;
        public const string SmsErrorMessage = "You have selected a contact preference which requires a valid phone number";

        public EditYourDetailsController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,
            IAddressSearchService addressSearchService, IDssReader dssReader, IDssWriter dssWriter, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, authService, documentService, config)
        {
            _addressSearchService = addressSearchService;
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
            
            if (!string.IsNullOrWhiteSpace(additionalData.FindAddress))
            {
                ModelState.Clear();
                await FindAddress(viewModel);
            }
            else if (!string.IsNullOrWhiteSpace(additionalData.SelectAddress))
            {
                ModelState.Clear();
                await GetSelectAddress(viewModel, additionalData);
            }
            else if (!string.IsNullOrWhiteSpace(additionalData.SaveDetails))
            {
                var customerDetails = await _dssReader.GetCustomerData(customer.CustomerId.ToString());
                if (IsValid(ModelState, viewModel))
                {
                    try
                    {
                        
                        string dateOfBirthDay = viewModel.Identity.PersonalDetails.DateOfBirthDay;
                        string dateOfBirthMonth = viewModel.Identity.PersonalDetails.DateOfBirthMonth;
                        string dateOfBirthYear = viewModel.Identity.PersonalDetails.DateOfBirthYear;

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
                        
                        var addressToUpdate = string.IsNullOrEmpty(viewModel.Identity.PersonalDetails.AddressId)
                            ? updatedDetails.Addresses.FirstOrDefault(x => string.IsNullOrEmpty(x.AddressId))
                            : updatedDetails.Addresses.FirstOrDefault(x =>
                                x.AddressId == viewModel.Identity.PersonalDetails.AddressId);

                        if (addressToUpdate != null)
                        {
                            //await _dssWriter.UpsertCustomerAddressData(addressToUpdate, updatedDetails.CustomerId);
                        }

                        updatedDetails.Contact.LastModifiedDate = DateTime.UtcNow.AddMinutes(-1);
                        
                        await _dssWriter.UpsertCustomerContactData(updatedDetails);

                        return new RedirectResult("/your-account/your-details", false);
                    }
                    catch (EmailAddressAlreadyExistsException)
                    {
                        ModelState.AddModelError("Identity.ContactDetails.ContactEmail", "Email address already in use");
                    }
                }
            }

            ViewModel.Items = viewModel.Items;
            ViewModel.Identity = viewModel.Identity;

            return await base.Body();
        }

        private bool IsValid(ModelStateDictionary modelState, EditDetailsCompositeViewModel viewModel)
        {
            if (viewModel.Identity.ContactDetails.ContactPreference == CommonEnums.Channel.Text)
            {
                var contact = viewModel.Identity.ContactDetails;
                if (string.IsNullOrEmpty(contact.MobileNumber) &&
                    string.IsNullOrEmpty(contact.TelephoneNumberAlternative) &&
                    string.IsNullOrEmpty(contact.HomeNumber))
                {
                    modelState.AddModelError("Identity.ContactDetails.MobileNumber", SmsErrorMessage);
                    modelState.AddModelError("Identity.ContactDetails.TelephoneNumberAlternative", SmsErrorMessage);
                    modelState.AddModelError("Identity.ContactDetails.HomeNumber", SmsErrorMessage);
                    return false;
                }
            }

            return modelState.IsValid;
        }

        private async Task FindAddress(EditDetailsCompositeViewModel viewModel)
        {
            var postalCode = viewModel.Identity.PersonalDetails.HomePostCode?.Trim();
            if (string.IsNullOrWhiteSpace(postalCode) || !ServiceCommon.IsValidUkPostcode(postalCode))
            {
                const string errorMessage = "Enter a valid postal code";
                ModelState.AddModelError("Identity.PersonalDetails.HomePostCode", errorMessage);
            }
            else
            {
                bool isFindAddressServiceOnError = false;
                try
                {
                    var addresses = (await _addressSearchService.GetAddresses(postalCode)).ToList();
                    if (addresses.Any())
                    {
                        viewModel.Items = addresses.ToList();
                    }
                }
                catch
                {

                    viewModel.Identity.PersonalDetails.FindAddressServiceResult = ErrorMessageServiceUnavailable;
                    isFindAddressServiceOnError = true;
                }

                if (viewModel.Items == null || viewModel.Items.Count == 0)
                {
                    viewModel.Items = new List<PostalAddressModel>() {new PostalAddressModel() {Error = 0}};

                    if (!isFindAddressServiceOnError)
                    {
                        string noResultsMessage =
                            "We do not have any address matching this postcode. Please enter your address details in the boxes provided.";
                        viewModel.Identity.PersonalDetails.FindAddressServiceResult = noResultsMessage;
                    }
                }

            }
        }

        private async Task GetSelectAddress(EditDetailsCompositeViewModel viewModel, EditDetailsAdditionalData additionalData)
        {
            if (additionalData.SelectAddress != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(additionalData.SelectedAddress.Id))
                    {
                        additionalData.SelectedAddress = await _addressSearchService.GetAddress(additionalData.SelectedAddress.Id);
                    }

                    if (additionalData.SelectedAddress != null)
                    {
                        viewModel.Identity.PersonalDetails.AddressLine1 = additionalData.SelectedAddress.Line1;
                        viewModel.Identity.PersonalDetails.AddressLine2 = additionalData.SelectedAddress.Line2;
                        viewModel.Identity.PersonalDetails.AddressLine3 = additionalData.SelectedAddress.Line3;
                        viewModel.Identity.PersonalDetails.AddressLine4 = additionalData.SelectedAddress.Line4;
                        viewModel.Identity.PersonalDetails.Town = additionalData.SelectedAddress.City;
                        viewModel.Identity.PersonalDetails.HomePostCode = additionalData.SelectedAddress.PostalCode;
                    }
                }
                catch
                {
                    viewModel.Identity.PersonalDetails.FindAddressServiceResult = ErrorMessageServiceUnavailable;
                }

            }
        }

        private EditDetailsAdditionalData GetEditDetailsAdditionalData(IFormCollection formCollection)
        {
            PostalAddressModel selectedAddress = null;

            var selectedAddressString = formCollection.FirstOrDefault(x =>
                string.Compare(x.Key, "select-address", StringComparison.CurrentCultureIgnoreCase) == 0).Value;

            if (!string.IsNullOrEmpty(selectedAddressString))
            {
                selectedAddress = JsonConvert.DeserializeObject<PostalAddressModel>(selectedAddressString);
            }

            return new EditDetailsAdditionalData
            {
                FindAddress = formCollection.FirstOrDefault(x => string.Compare(x.Key, "findAddress", StringComparison.CurrentCultureIgnoreCase) == 0).Value,
                SaveDetails = formCollection.FirstOrDefault(x => string.Compare(x.Key, "saveDetails", StringComparison.CurrentCultureIgnoreCase) == 0).Value,
                SelectAddress = formCollection.FirstOrDefault(x => string.Compare(x.Key, "selectAddress", StringComparison.CurrentCultureIgnoreCase) == 0).Value,
                MarketResearchOptIn = !string.IsNullOrEmpty(formCollection.FirstOrDefault(x => string.Compare(x.Key, "marketResearchOptIn", StringComparison.CurrentCultureIgnoreCase) == 0).Value),
                MarketingOptIn = !string.IsNullOrEmpty(formCollection.FirstOrDefault(x => string.Compare(x.Key, "marketingOptIn", StringComparison.CurrentCultureIgnoreCase) == 0).Value),
              SelectedAddress = selectedAddress
            };

        }

        private Customer GetUpdatedCustomerDetails(Customer customer, CitizenIdentity identity)
        {
            customer.Contact.PreferredContactMethod = identity.ContactDetails.ContactPreference;
            customer.Contact.HomeNumber = identity.ContactDetails.HomeNumber;
            customer.Contact.MobileNumber = identity.ContactDetails.MobileNumber;
            customer.Contact.AlternativeNumber = identity.ContactDetails.TelephoneNumberAlternative;

            customer.OptInMarketResearch = identity.MarketingPreferences.MarketResearchOptIn;
            customer.OptInUserResearch = identity.MarketingPreferences.MarketingOptIn;

            customer.DateofBirth = identity.PersonalDetails.DateOfBirth;
            customer.FamilyName = identity.PersonalDetails.FamilyName;
            customer.Gender = identity.PersonalDetails.Gender;
            customer.GivenName = identity.PersonalDetails.GivenName;
            customer.Title = identity.PersonalDetails.Title;

            if (customer.Addresses == null)
            {
                customer.Addresses = new List<Address>();
            }

            var address = customer.Addresses.FirstOrDefault(x => x.AddressId == identity.PersonalDetails.AddressId);

            if (!string.IsNullOrEmpty(identity.PersonalDetails.AddressId)
                    && address != null)
            {
                address.Address1 = identity.PersonalDetails.AddressLine1;
                address.Address2 = identity.PersonalDetails.AddressLine2;
                address.Address3 = identity.PersonalDetails.AddressLine3;
                address.Address4 = identity.PersonalDetails.AddressLine4;
                address.Address5 = identity.PersonalDetails.Town;
                address.PostCode = identity.PersonalDetails.HomePostCode;
                address.LastModifiedDate = DateTimeOffset.Now;
                address.EffectiveFrom = DateTimeOffset.Now;
            }
            else
            {
                customer.Addresses.Add(new Address
                {
                    Address1 = identity.PersonalDetails.AddressLine1,
                    Address2 = identity.PersonalDetails.AddressLine2,
                    Address3 = identity.PersonalDetails.AddressLine3,
                    Address4 = identity.PersonalDetails.AddressLine4,
                    Address5 = identity.PersonalDetails.Town,
                    PostCode = identity.PersonalDetails.HomePostCode,
                    LastModifiedDate = DateTimeOffset.Now,
                    EffectiveFrom = DateTimeOffset.Now,

                });
            }

            return customer;
        }

        private CitizenIdentity MapCustomerToCitizenIdentity(Customer customer)
        {
            var currentAddress = customer.Addresses?.OrderByDescending(x => x.EffectiveFrom).FirstOrDefault(x =>
               x.EffectiveFrom.HasValue && x.EffectiveFrom.Value.Date <= DateTime.Now &&
               (x.EffectiveTo == null || x.EffectiveTo.Value.Date >= DateTime.Now));

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
                    AddressLine1 = currentAddress?.Address1,
                    AddressLine2 = currentAddress?.Address2,
                    AddressLine3 = currentAddress?.Address3,
                    AddressLine4 = currentAddress?.Address4,
                    Town = currentAddress?.Address5,
                    HomePostCode = currentAddress?.PostCode,
                    AddressId = currentAddress?.AddressId,
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