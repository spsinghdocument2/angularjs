using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    // create web api into create account
    public class RegisterController : ApiController
    {
        // Get interface Register
        private readonly IRegister IRegister;

        public RegisterController(IRegister iRegister)
        {
            // Create object Register OR Create Account
            IRegister = iRegister;
           
        }

        #region  Registration 
        //Post the Registration form  
        public HttpResponseMessage POST(RegisterEntity registerEntity)
        {
            try
            {
                string message = string.Empty;
                if (registerEntity == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }

                if (string.IsNullOrWhiteSpace(registerEntity.AccountNumber) || string.IsNullOrWhiteSpace(registerEntity.Answer) || string.IsNullOrWhiteSpace(registerEntity.Email) || string.IsNullOrWhiteSpace(registerEntity.FullName) || string.IsNullOrWhiteSpace(registerEntity.Password))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    // successfully registered  into create account
                    message = IRegister.userRegister(registerEntity);

                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Register Controller in Post Action Method", ex);
                return null;
            }
        }
        #endregion


    }
}
