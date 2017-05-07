using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    public class AuthenticateController : ApiController
    {
        // Get interface Authenticate
        private readonly IAuthenticate IAuthenticate;

        public AuthenticateController(IAuthenticate iAuthenticate)
        {
            // Set object Authenticate class
            IAuthenticate = iAuthenticate;
           
        }
        //Post the create account  Verify
        #region  Authenticate Account Verify
        public HttpResponseMessage POST(AuthenticateVerifyEntity authenticateVerifyEntity)
        {
            try
            {
                // string error message post
                string message = string.Empty;
                // Model post verify account
                AuthenticateConfirmationEntity authenticateConformationEntity = null;

                if (authenticateVerifyEntity == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }

                if (string.IsNullOrWhiteSpace(authenticateVerifyEntity.AccountNumber) || string.IsNullOrWhiteSpace(authenticateVerifyEntity.LastName))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {

                    authenticateConformationEntity = IAuthenticate.AuthenticateConformationUser(authenticateVerifyEntity);

                    return Request.CreateResponse(HttpStatusCode.OK, authenticateConformationEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Authenticate Controller Authenticate Post Action Method", ex);
                return null;
            }  

        }
        #endregion
    }
}
