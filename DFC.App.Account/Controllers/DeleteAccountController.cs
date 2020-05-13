using System;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Account.Controllers
{
    public class DeleteAccountController : CompositeSessionController<DeleteAccountCompositeViewModel>
    {
        private readonly IDssWriter _dssService;
        public DeleteAccountController(IOptions<CompositeSettings> compositeSettings, IDssWriter dssService, IAuthService authService)
            : base(compositeSettings, authService)
        {
            _dssService = dssService;
        }

        [HttpGet,HttpPost]
        public async Task<IActionResult> Body(DeleteAccountCompositeViewModel model)
        {

            
             /*
             Logic for deleting the account
                var request = new DeleteAccountRequest
                {
                    CitizenId = model.citizenId,
                    Password = model.Password,
                    Reason = model.Reason
                };
                //Call DSS Service Endpoint 
                var deleteRequest = Task.Run(async () => await ServiceClient.DeleteAccountAsync(request)).Result;

                 if (deleteRequest.Success)
                {
                   Response.Redirect("/home/signout", true);
                }

                string deleteRequestErrorMessage = string.Empty;
                string currentPageUrl = Request.Url.AbsolutePath;

                switch (deleteRequest.ErrorMessage.ToLowerInvariant())
                {
                    case ServiceConstants.CompareValues.invalidpassword:
                        var validationSet =
                            ValidationRulesProvider.GetValidationSetPerPageAndPropertyName(currentPageUrl,
                                nameof(model.Password));
                        if (validationSet != null && !string.IsNullOrEmpty(validationSet.CustomValidationErrorMessage))
                        {
                            deleteRequestErrorMessage = validationSet.CustomValidationErrorMessage;
                        }
                        else
                        {
                            deleteRequestErrorMessage = "Invalid password.";
                        }

                        break;
                    case ServiceConstants.CompareValues.genericerror:
                        var validationSetGeneric =
                            ValidationRulesProvider.GetValidationSetPerPageAndPropertyName(currentPageUrl,
                                ServiceConstants.MessageTypes.GenericError);
                        if (validationSetGeneric != null &&
                            !string.IsNullOrEmpty(validationSetGeneric.CustomValidationErrorMessage))
                        {
                            deleteRequestErrorMessage = validationSetGeneric.CustomValidationErrorMessage;
                        }
                        else
                        {
                            deleteRequestErrorMessage = "Could not close account. Please try again later";
                        }

                        break;
                    default:
                        break;
                }

                ModelState.AddModelError(nameof(model.Password), deleteRequestErrorMessage); 
            */
            

             await _dssService.DeleteCustomer();
             return View(ViewModel);
        }

    }
}
