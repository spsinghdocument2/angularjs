
using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    public class ForgotPasswordController : ApiController
    {
        ForgotConformationEntity forgotConformationEntity = null;
        // Get interface Forgot
        private readonly IForgotPassword IForgot;
        public ForgotPasswordController(IForgotPassword iForgot)
        {
            // Create object Forgot class
            IForgot = iForgot;
        }
        //Post the Forgot Verify function
        #region  Forgot Verify
        [HttpPost]
        public HttpResponseMessage POST(ForgotConformationEntity ForgotParameter)
        {
            try
            {
                string message = string.Empty;
                if (ForgotParameter == null)
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);

                }

                // Model class (ForgotConformationEntity) create object post Forgot Verify
                forgotConformationEntity = new ForgotConformationEntity();

                if (string.IsNullOrWhiteSpace(ForgotParameter.AccountNumber))
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    //Pass the parameter value into the forgot Conformation
                    forgotConformationEntity = IForgot.verify(ForgotParameter.AccountNumber);

                    //send web api verify account
                    return Request.CreateResponse(HttpStatusCode.OK, forgotConformationEntity);
                }
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Forget Password Controller in Forget Password Verify Action Method", ex);
                return null;
            } 

        }
        #endregion
        

        //Send mail Temporary forgot Password
        #region  Forgot send mail Temporary Password
        [HttpPost]
        public HttpResponseMessage ForgotTemporaryPasswordSendMail(ChangePasswordEntity models)
        {
            try
            {
                string message = string.Empty;
                if (models == null)
                {
                    // Error message send forgot ConformationEntity Model Empty
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }

                // Error message send forgot web api
                if (string.IsNullOrWhiteSpace(models.AccountNumber) ||  string.IsNullOrWhiteSpace(models.FullName) || string.IsNullOrWhiteSpace(models.Email))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    // Send mail Conformation
                    message = IForgot.TemporarypasswordSendMail(models);


                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Forget Passowrd Controller Send Mail Method", ex);
                return null;
            }

        }
        #endregion



    }
}
