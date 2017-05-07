using MAF.BAL;
using MAF.ENTITY;
using MAF.SolutionByText;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
     [RoutePrefix("api/UserProfile")]
    public class UserProfileController : ApiController
    {
            // Get interface Account Information
            private readonly IUserProfile IUserProfile;
            private readonly IGeneralService generalService;
          //This property datamember is used to create object of Account Information
            UserProfileEntity userprofileEntity = null;

            public UserProfileController(IUserProfile iUserProfile, IGeneralService generalService)
            {
                // Create object Payonline(Account Information)
                IUserProfile = iUserProfile;
                this.generalService = generalService;
            }

            #region User Account Information Get
            //Get the Account Information  function
         [Authorize]
            [HttpGet]
            [Route("GetUserProfile/{accountNumber:int}")]
            public IHttpActionResult GetUserProfile(int accountNumber)
            {
                try
                {
                //  This private datamember is used to create object of Account Information
                userprofileEntity = new UserProfileEntity();


                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);

                }
                else
                {
                    // search account Information get
                    userprofileEntity = IUserProfile.GetUserProfile(Convert.ToString(accountNumber));
                    //send web api  account Information
                    return Ok(userprofileEntity);
                }
            }
                catch (Exception ex)
                {
                    MAF.BAL.Logger.WriteErrorLog("Error in User Profile Controller in Get User Details Action Method", ex);
                    return null;
                }
            }

            // This Post return the Payment Confirmation view page.
            [HttpPost]
            [Route("UpdateUserInfo")]
            public async Task<HttpResponseMessage> UpdateUserInfo()
            {
                try
                {
                    ResponseMessage responseMessage = new ResponseMessage();

                    Dictionary<string, string> extensionLookup = new Dictionary<string, string>()
                    {
                        {"image/png", ".png"},
                        {"image/jpeg", ".jpeg"},
                        {"image/gid", ".gif"}
                    };

                    if (!Request.Content.IsMimeMultipartContent())
                    {
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                    }

                    var root = HttpContext.Current.Server.MapPath("~/images/uploads");
                    var provider = new MultipartFormDataStreamProvider(root);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);
                    if (result.FormData["model"] == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }

                    var modelData = result.FormData["model"];

                    UserProfileEntity model = JsonConvert.DeserializeObject<UserProfileEntity>(modelData);

                    //Validate text numbers
                    if (!string.IsNullOrEmpty(model.TextNumber))
                    {

                        //Do carrier lookup to validate phone number
                        responseMessage = generalService.GetCarrierLookup(model.TextNumber);

                        if (!responseMessage.IsSuccess)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);
                        }
                    }
                    if (!string.IsNullOrEmpty(model.TextNumber2))
                    {
                        //Do carrier lookup to validate phone number
                        responseMessage = generalService.GetCarrierLookup(model.TextNumber2);

                        if (!responseMessage.IsSuccess)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);
                        }
                    }

                    //get the files
                    foreach (var fileData in result.FileData)
                    {
                        if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                        }
                        string fileName = fileData.Headers.ContentDisposition.FileName;

                        fileName = Guid.NewGuid().ToString() + extensionLookup[fileData.Headers.ContentType.ToString()];
                        model.Image = "~/images/uploads/" + fileName;
                        File.Move(fileData.LocalFileName, Path.Combine(root, fileName));
                    }
                    string message = string.Empty;
                    if (model == null)
                    {
                        // Error message send
                        message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                        //  return (HttpStatusCode.BadRequest, message);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                    }

                    if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.CellNumber) || string.IsNullOrWhiteSpace(model.Address1) || string.IsNullOrWhiteSpace(model.AccountNumber) || string.IsNullOrWhiteSpace(model.City) || string.IsNullOrWhiteSpace(model.State) || string.IsNullOrWhiteSpace(model.Zip))
                    {
                        // Error message send
                        message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                    }
                    else
                    {
                        IUserProfile.UpdateUserProfile(model.Email, model.CellNumber, model.TextNumber, model.TextNumber2, model.Address1, model.AccountNumber, model.Image, model.Address2, model.City, model.State, model.Zip);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    MAF.BAL.Logger.WriteErrorLog("Error in User Profile Controller Save Details Action Method", ex);
                    return null;
                }
            }
            // This Post return the UPDATE  SECURITY QUESTIONS
            
            #endregion
            #region UPDATE  SECURITY QUESTIONS
         [Authorize]
         [HttpPost]
            [Route("UpdateSecurityQuestion")]
            public IHttpActionResult UpdateSecurityQuestion(ForgotConformationEntity model)
            {
                try
                {
                string message = string.Empty;
                if (model == null)
                {
                     // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);

                }


                if (string.IsNullOrWhiteSpace(model.AccountNumber) || string.IsNullOrWhiteSpace(model.SecurityID) || string.IsNullOrWhiteSpace(model.Answer))
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);
                }
                else
                {
                    message = IUserProfile.UpdateSecurityQuestion(model);
                    return Ok(message);
                }
            }
                catch (Exception ex)
                {
                    MAF.BAL.Logger.WriteErrorLog("Error in User Profile Controller in Update Security Question Action Method", ex);
                    return null;
                }
            }

        #endregion
    }

}