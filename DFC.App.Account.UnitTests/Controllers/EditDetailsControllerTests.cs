using System;
using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.Interfaces;
using DFC.App.Account.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Models;
using Microsoft.Extensions.Primitives;
using NSubstitute.ExceptionExtensions;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class EditDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IAddressSearchService _addressSearchService;
        private IDssReader _dssReader;
        private IDssWriter _dssWriter;


        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _addressSearchService = Substitute.For<IAddressSearchService>();
            _dssWriter = Substitute.For<IDssWriter>();
            _dssReader = Substitute.For<IDssReader>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService, _dssReader, _dssWriter);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer());

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


        [Test]
        public async Task WhenBodyCalled_ThenDssCalled()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService, _dssReader, _dssWriter);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer());

            await controller.Body();
            await _dssReader.Received().GetCustomerData(Arg.Any<string>());
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
            editViewModel.SelectedAddress = new List<PostalAddressModel>();
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

        [Test]
        public async Task WhenSaveDataPostedAndFormHasErrors_ThenDateShouldNotSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
            };
            controller.ModelState.AddModelError("email","email");
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer());

            var result = await controller.Body(new EditDetailsCompositeViewModel
            {
                Identity = new CitizenIdentity
                {
                    ContactDetails = new ContactDetails
                    {
                        ContactEmail = "7777777"
                    }
                }
            },new FormCollection(new Dictionary<string, StringValues>
            {
                {"saveDetails", "saveDetails"}
            })) as ViewResult;

            _dssWriter.Received(0);

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenSaveDataPostedAndFormIsValid_ThenDateShouldBeSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact()
            });

            var result = await controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"saveDetails", "saveDetails"}
            })) as ViewResult;
            
            _dssWriter.Received(3);
        }

        [Test]
        public async Task WhenSaveDataPostedAndFormIsValidAndEmailIsUpdated_ThenDateShouldBeSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            var result = await controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"saveDetails", "saveDetails"}
            })) as RedirectResult;

            result.Should().NotBeNull();
            result.Url.Should().Be("/your-account/your-details?logout=true");
        }

        [Test]
        public async Task WhenFindAddressPostedAndPostcodeIsNotValid_ThenAddressServiceNotCalledAndModelIsReturnedWithError()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            var result = await controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"findAddress", "findAddress"}
            })) as ViewResult;

            result.Should().NotBeNull();
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task WhenFindAddressPostedAndFormIsValid_ThenAddressServiceCalled()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddresses(Arg.Any<string>()).ReturnsForAnyArgs(new List<PostalAddressModel>
            {
                new PostalAddressModel
                {
                    Line1 = "test"
                }
            });

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"findAddress", "findAddress"}
            })) as ViewResult;

            result.Should().NotBeNull();
            ((EditDetailsCompositeViewModel) result.ViewData.Model).Items.Any().Should().BeTrue();
        }

        [Test]
        public async Task WhenFindAddressPostedAndFormIsValidAndAddressServiceIsDown_ThenReturnServiceDownMessage()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddresses(Arg.Any<string>()).ThrowsForAnyArgs(new System.Exception());

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"findAddress", "findAddress"}
            })) as ViewResult;

            result.Should().NotBeNull();
            ((EditDetailsCompositeViewModel)result.ViewData.Model).Identity.PersonalDetails.FindAddressServiceResult.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task WhenFindAddressPostedAndNoAddressesAreFound_ThenReturnNoAddressFoundMessage()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddresses(Arg.Any<string>()).ReturnsForAnyArgs(new List<PostalAddressModel>());

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";
            viewModel.Items = null;

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"findAddress", "findAddress"}
            })) as ViewResult;

            result.Should().NotBeNull();
            ((EditDetailsCompositeViewModel)result.ViewData.Model).Identity.PersonalDetails
                .FindAddressServiceResult.Should().Be("We do not have any address matching this postcode. Please enter your address details in the boxes provided.");
        }

        [Test]
        public async Task WhenSelectAddressPosted_ThenAddressServiceCalled()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddresses(Arg.Any<string>()).ReturnsForAnyArgs(new List<PostalAddressModel>
            {
                new PostalAddressModel
                {
                    Line1 = "test"
                }
            });

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"selectAddress", "selectAddress"},
                {"select-address", "{\"id\":\"test\"}" }
            })) as ViewResult;

            await _addressSearchService.Received().GetAddress(Arg.Any<string>());

        }
        
        [Test]
        public async Task WhenSelectAddressPosted_ThenReturnViewModelWithSelectedAddress()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddress(Arg.Any<string>()).ReturnsForAnyArgs(new PostalAddressModel
            {
                PostalCode = "test"
            });

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"selectAddress", "selectAddress"},
                {"select-address", "{\"id\":\"test\"}" }
            })) as ViewResult;

            result.Should().NotBeNull();
            ((EditDetailsCompositeViewModel) result.ViewData.Model).Identity.PersonalDetails.HomePostCode.Should()
                .Be("test");
        }

        [Test]
        public async Task WhenSelectAddressPostedAndAddressServiceIsDown_ThenReturnServiceDownMessage()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact
                {
                    EmailAddress = "test"
                }
            });

            _addressSearchService.GetAddress(Arg.Any<string>()).ThrowsForAnyArgs(new System.Exception());

            var viewModel = GetViewModel();
            viewModel.Identity.PersonalDetails.HomePostCode = "lg23hg";

            var result = await controller.Body(viewModel, new FormCollection(new Dictionary<string, StringValues>
            {
                {"selectAddress", "selectAddress"},
                {"select-address", "{\"id\":\"test\"}" }
            })) as ViewResult;

            result.Should().NotBeNull();
            ((EditDetailsCompositeViewModel)result.ViewData.Model).Identity.PersonalDetails.FindAddressServiceResult.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void WhenGetErrorClassCalledWithError_ReturnClass()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ModelState.AddModelError("test","test");
            var viewModel = new EditDetailsCompositeViewModel();
           viewModel.GetErrorClass("test", controller.ViewData.ModelState).Should().NotBeNullOrEmpty();
        }

        [Test]
        public void WhenGetErrorClassCalledWithError_ReturnEmptyString()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _addressSearchService,
                _dssReader, _dssWriter)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            
            var viewModel = new EditDetailsCompositeViewModel();
            viewModel.GetErrorClass("test", controller.ViewData.ModelState).Should().BeNullOrEmpty();
        }
        private EditDetailsCompositeViewModel GetViewModel()
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

            return editViewModel;
        }
    }
}
