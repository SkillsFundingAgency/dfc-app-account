using System;
using DFC.App.Account.Application.Common.Enums;
using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Controllers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
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
using DFC.APP.Account.Data.Common;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Services.DSS.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NSubstitute.ExceptionExtensions;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.UnitTests.Controllers
{
    public class EditDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private IAuthService _authService;
        private IDssReader _dssReader;
        private IDssWriter _dssWriter;
        private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;
        private ISharedContentRedisInterface _sharedContentRedisInterface;

        [SetUp]
        public void Init()
        {
            _documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            var inMemorySettings = new Dictionary<string, string> {
                {Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _compositeSettings = Options.Create(new CompositeSettings());
            _authService = Substitute.For<IAuthService>();
            _dssWriter = Substitute.For<IDssWriter>();
            _dssReader = Substitute.For<IDssReader>();
            _sharedContentRedisInterface = Substitute.For<ISharedContentRedisInterface>();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _dssReader, _dssWriter, _config, _sharedContentRedisInterface);
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
            var controller = new EditYourDetailsController(_compositeSettings, _authService, _dssReader, _dssWriter, _config, _sharedContentRedisInterface);
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
                    HomeNumber = "8675309",
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
                    DateOfBirth = null,
                    DateOfBirthDay = "05",
                    DateOfBirthMonth = "Oct",
                    DateOfBirthYear = "1990",
                }
            };
            var identity = editViewModel.Identity;

            editViewModel.Should().NotBeNull();
            identity.Should().NotBeNull();
        }
        
        [Test]
        public async Task WhenSaveDataPostedAndFormHasErrors_ThenDateShouldNotSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
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
        public async Task WhenSaveDataPostedAsSmsAndAllValuesAreEmpty_ThenDateShouldNotSaved()
        {
            var customer = new Customer() { CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c") };
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter,  _config, _sharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer());

            var result = await controller.Body(new EditDetailsCompositeViewModel
            {
                Identity = new CitizenIdentity
                {
                    ContactDetails = new ContactDetails
                    {
                        ContactEmail = "7777777",
                        ContactPreference = CommonEnums.Channel.Text
                    }
                }
            }, new FormCollection(new Dictionary<string, StringValues>
            {
                {"saveDetails", "saveDetails"}
            })) as ViewResult;

            _dssWriter.Received(0);

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


        [Test]
        public async Task WhenSaveDataPostedWithMobilePreferenceAndFormIsValid_ThenDateShouldBeSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            _dssReader.GetCustomerData(Arg.Any<string>()).ReturnsForAnyArgs(new Customer
            {
                Contact = new Contact()
            });

            EditDetailsCompositeViewModel editDetailVm = GetViewModel();
            editDetailVm.Identity.ContactDetails.ContactPreference = CommonEnums.Channel.Email;
            var result = await controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"saveDetails", "saveDetails"}
            })) as ViewResult;
            
            _dssWriter.Received(3);
        }

        [Test]
        public async Task WhenSaveDataPostedAndFormIsValid_ThenDateShouldBeSaved()
        {
            var customer = new Customer() {CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c")};
            _authService.GetCustomer(Arg.Any<ClaimsPrincipal>()).Returns(customer);
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
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
        public void WhenGetErrorClassCalledWithError_ReturnClass()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
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
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            
            var viewModel = new EditDetailsCompositeViewModel();
            viewModel.GetErrorClass("test", controller.ViewData.ModelState).Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenGetFormGroupErrorClassCalledWithError_ReturnClass()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ModelState.AddModelError("test", "test");
            var viewModel = new EditDetailsCompositeViewModel();
            viewModel.GetFormGroupErrorClass("test", controller.ViewData.ModelState).Should().NotBeNullOrEmpty();
        }

        [Test]
        public void WhenGetFormGroupErrorClassCalledWithError_ReturnEmptyString()
        {
            var controller = new EditYourDetailsController(_compositeSettings, _authService,
                _dssReader, _dssWriter, _config, _sharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var viewModel = new EditDetailsCompositeViewModel();
            viewModel.GetFormGroupErrorClass("test", controller.ViewData.ModelState).Should().BeNullOrEmpty();
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
                    HomeNumber = "8675309",
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
                    DateOfBirth = null,
                    DateOfBirthDay = "05",
                    DateOfBirthMonth = "Oct",
                    DateOfBirthYear = "1990",
                }
            };

            return editViewModel;
        }
    }
}
