using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using WebAPIAndAngular.Models;

namespace MAF.WEBAPI.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        // Get interface Login
        private readonly ILogin ILogin;

        public LoginController(ILogin iLogin)
        {
            // Create object Login
            ILogin = iLogin;
           
        }
        //Post the Login Account Verify
        #region  Login Account Verify
        [HttpPost]
        [Route("LoginUser")]
        public HttpResponseMessage POST(LoginEntity loginEntiy)
        {
            try
            {
                LoginEntity objLoginEntity = new LoginEntity();
                if (string.IsNullOrWhiteSpace(loginEntiy.AccountNumber) || string.IsNullOrWhiteSpace(loginEntiy.Email) || string.IsNullOrWhiteSpace(loginEntiy.Password))
                {
                    // Error message send                    
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, MAF.BAL.ResourceFile.Common.RequestInvalid);
                }
                else
                {
                    //Pass the parameter account number , Email and password into the Login 
                    objLoginEntity = ILogin.UserLogin(loginEntiy);

                    // Send web api  conformation into the Login
                    return Request.CreateResponse(HttpStatusCode.OK, objLoginEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Login Controller Login Post Action Method", ex);
                return null;
            }

        }
        #endregion


       
    }
}
