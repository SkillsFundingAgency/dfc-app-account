using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Exceptions;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public EditYourDetailsController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,
            IAddressSearchService addressSearchService, IDssReader dssReader, IDssWriter dssWriter)
            : base(compositeSettings, authService)
        {
            _addressSearchService = addressSearchService;
            _dssReader = dssReader;
            _dssWriter = dssWriter;
        }

        [Microsoft.AspNetCore.Mvc.Route("/body/edit-your-details")]
        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            var customerDetails = await _dssReader.GetCustomerData(customer.CustomerId.ToString());
            ViewModel.Identity = MapCustomerToCitizenIdentity(customerDetails);

            return await base.Body();
        }

        [Microsoft.AspNetCore.Mvc.Route("/body/edit-your-details")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
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
                if (ModelState.IsValid)
                {
                    try
                    {

                        string dateOfBirthDay = viewModel.Identity.PersonalDetails.DateOfBirthDay;
                        string dateOfBirthMonth = viewModel.Identity.PersonalDetails.DateOfBirthMonth;
                        string dateOfBirthYear = viewModel.Identity.PersonalDetails.DateOfBirthYear;

                        DateTime dateOfBirth = default(DateTime);
                        CultureInfo enGb = new CultureInfo("en-GB");
                        string dob = string.Empty;
                        if (!string.IsNullOrEmpty(dateOfBirthDay) && !string.IsNullOrEmpty(dateOfBirthMonth) &&
                            !string.IsNullOrEmpty(dateOfBirthYear))
                        {
                            dob = string.Format("{0}/{1}/{2}", dateOfBirthDay.PadLeft(2, '0'),
                                dateOfBirthMonth.PadLeft(2, '0'), dateOfBirthYear.PadLeft(4, '0'));
                        }

                        if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal,
                            out dateOfBirth))
                        {
                            viewModel.Identity.PersonalDetails.DateOfBirth = dateOfBirth;
                        }
                        else
                        {
                            viewModel.Identity.PersonalDetails.DateOfBirth = null;
                        }

                        var existingEmail = customerDetails.Contact.EmailAddress;
                        var updatedDetails = GetUpdatedCustomerDetails(customerDetails, viewModel.Identity);

                        await _dssWriter.UpdateCustomerData(updatedDetails);

                        var addressToUpdate = string.IsNullOrEmpty(viewModel.Identity.PersonalDetails.AddressId)
                            ? updatedDetails.Addresses.FirstOrDefault(x => string.IsNullOrEmpty(x.AddressId))
                            : updatedDetails.Addresses.FirstOrDefault(x =>
                                x.AddressId == viewModel.Identity.PersonalDetails.AddressId);

                        if (addressToUpdate != null)
                        {
                            await _dssWriter.UpsertCustomerAddressData(addressToUpdate, updatedDetails.CustomerId);
                        }

                        customerDetails.Contact.LastModifiedDate = DateTime.UtcNow;
                        await _dssWriter.UpsertCustomerContactData(updatedDetails);

                        if (existingEmail != viewModel.Identity.ContactDetails.ContactEmail)
                        {
                            //Auth endpoint needed here
                            return new RedirectResult("/your-account/your-details?logout=true", false);
                        }

                        return new RedirectResult("/your-account/your-details", false);
                    }
                    catch (EmailAddressAlreadyExistsException ex)
                    {
                        ModelState.AddModelError("Identity.ContactDetails.ContactEmail", "Email address already in use");
                    }
                }
            }

            ViewModel.Items = viewModel.Items;
            ViewModel.Identity = viewModel.Identity;

            return await base.Body();
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
                        viewModel.Identity.PersonalDetails.AddressLine5 = additionalData.SelectedAddress.Line5;
                        viewModel.Identity.PersonalDetails.HomePostCode = additionalData.SelectedAddress.PostalCode;
                        viewModel.Identity.PersonalDetails.Town = additionalData.SelectedAddress.City;
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

            var SelectedAddressString = formCollection.FirstOrDefault(x =>
                string.Compare(x.Key, "select-address", StringComparison.CurrentCultureIgnoreCase) == 0).Value;

            if (!string.IsNullOrEmpty(SelectedAddressString))
            {
                selectedAddress = JsonConvert.DeserializeObject<PostalAddressModel>(SelectedAddressString);
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
            customer.Contact.EmailAddress = identity.ContactDetails.ContactEmail;
            customer.Contact.PreferredContactMethod = identity.ContactDetails.ContactPreference;
            customer.Contact.HomeNumber = identity.ContactDetails.TelephoneNumber;
            customer.Contact.AlternativeNumber = identity.ContactDetails.TelephoneNumberAlternative;

            customer.OptInMarketResearch = identity.MarketingPreferences.MarketResearchOptIn;
            customer.OptInMarketResearch = identity.MarketingPreferences.MarketingOptIn;

            customer.DateofBirth = identity.PersonalDetails.DateOfBirth;
            customer.FamilyName = identity.PersonalDetails.FamilyName;
            customer.Gender = identity.PersonalDetails.Gender;
            customer.GivenName = identity.PersonalDetails.GivenName;
            customer.Title = identity.PersonalDetails.Title;

            if (customer.Addresses == null)
            {
                customer.Addresses = new List<Address>();
            }


            if (!string.IsNullOrEmpty(identity.PersonalDetails.AddressId)
                    && customer.Addresses.FirstOrDefault(x => x.AddressId == identity.PersonalDetails.AddressId) != null)
            {
                var address = customer.Addresses.FirstOrDefault(x => x.AddressId == identity.PersonalDetails.AddressId);

                address.Address1 = identity.PersonalDetails.AddressLine1;
                address.Address2 = identity.PersonalDetails.AddressLine2;
                address.Address3 = identity.PersonalDetails.AddressLine3;
                address.Address4 = identity.PersonalDetails.AddressLine4;
                address.Address5 = identity.PersonalDetails.AddressLine5;
                address.PostCode = identity.PersonalDetails.HomePostCode;
                address.AlternativePostCode = identity.PersonalDetails.AlternativePostCode;
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
                    Address5 = identity.PersonalDetails.AddressLine5,
                    PostCode = identity.PersonalDetails.HomePostCode,
                    AlternativePostCode = identity.PersonalDetails.AlternativePostCode,
                    LastModifiedDate = DateTimeOffset.Now,
                    EffectiveFrom = DateTimeOffset.Now,

                });
            }

            return customer;
        }

        private CitizenIdentity MapCustomerToCitizenIdentity(Customer customer)
        {
            var currentAddress = customer.Addresses?.OrderByDescending(x => x.EffectiveFrom).FirstOrDefault(x =>
               x.EffectiveFrom.Date <= DateTime.Now &&
               (x.EffectiveTo == null || x.EffectiveTo.Value.Date >= DateTime.Now));

            return ViewModel.Identity = new CitizenIdentity
            {
                ContactDetails = new ContactDetails
                {
                    ContactEmail = customer.Contact?.EmailAddress,
                    ContactPreference = customer.Contact?.PreferredContactMethod ?? CommonEnums.Channel.Email,
                    TelephoneNumber = customer.Contact?.HomeNumber,
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
                    AddressLine5 = currentAddress?.Address5,
                    HomePostCode = currentAddress?.PostCode,
                    AddressId = currentAddress?.AddressId,
                    AlternativePostCode = currentAddress?.AlternativePostCode,
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