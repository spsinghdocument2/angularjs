using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    public class ChangePasswordController : ApiController
    {
        // Get interface ChangePassword
        private readonly IChangePassword IChangePassword;
        ChangePasswordEntity changePasswordEntity = null;
        public ChangePasswordController(IChangePassword iChangePassword)
        {
            // Set object ChangePassword class
            IChangePassword = iChangePassword;
        }

        //Post the change password  Verify
        #region  change password verify 
        [HttpPost]
       
        public HttpResponseMessage changePasswordVerify(ChangePasswordVerifyEntity changegePasswordVerifyEntity)
        {
            try
            {
                // Model class create object post changege Password verify account
                changePasswordEntity = new ChangePasswordEntity();
                // string error message post
                string message = string.Empty;
                if (changegePasswordVerifyEntity == null)
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }

                if (string.IsNullOrWhiteSpace(changegePasswordVerifyEntity.AccountNumber))
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    //Pass the parameter value into the ChangegePassword verify
                    changePasswordEntity = IChangePassword.verify(changegePasswordVerifyEntity.AccountNumber, changegePasswordVerifyEntity.Password);


                    return Request.CreateResponse(HttpStatusCode.OK, changePasswordEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Change Password Controller in Change Password verify Action Method", ex);
                return null;
            }  
        }
        #endregion
        //This function password Change
        #region password Change
        public HttpResponseMessage passwordReset(ChangePasswordEntity changegePasswordEntity)
        {
            try
            {
                string message = string.Empty;
                if (changegePasswordEntity == null)
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);

                }

                if (string.IsNullOrWhiteSpace(changegePasswordEntity.AccountNumber) || string.IsNullOrWhiteSpace(changegePasswordEntity.NewPassword) || string.IsNullOrWhiteSpace(changegePasswordEntity.FullName) || string.IsNullOrWhiteSpace(changegePasswordEntity.Email))
                {
                    // string error message post
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    //Pass the parameter value into the ChangegePassword 
                    message = IChangePassword.ResetPassword(changegePasswordEntity);


                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }             
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Change Password Controller in Change Password Action Method", ex);
                return null;
            }  

        }

        #endregion

    
    }
}
