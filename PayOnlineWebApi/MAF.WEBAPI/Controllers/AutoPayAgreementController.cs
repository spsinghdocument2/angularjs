using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
     [RoutePrefix("api/AutoPayAgreement")]
    public class AutoPayAgreementController : ApiController
    {
         //Intialize interface members
        private readonly IAutoPayAgreement autoPayAgreement;
        private readonly IUtility utility;

        public AutoPayAgreementController(IAutoPayAgreement autoPayAgreement, IUtility utility)
        {
            this.autoPayAgreement = autoPayAgreement;
            this.utility = utility;
        }

         /// <summary>
         /// Method to get details for AutoPay Payment method and fee based on payment method
         /// </summary>
         /// <param name="accountNumber"></param>
         /// <returns></returns>
         [HttpGet]
         [Route("GetAutoPayPaymentMethod/{accountNumber}")]
         public HttpResponseMessage GetAutoPayPaymentMethodDetails(string accountNumber)
         {
             try
             {
                 string paymentMethod = string.Empty, fee = string.Empty;
                 if (string.IsNullOrWhiteSpace(accountNumber))
                 {
                     return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Account Number cannot be blank.");
                 }
                 else
                 {
                     paymentMethod = autoPayAgreement.GetAutoPayPaymentMethod(accountNumber);
                     if (!string.IsNullOrEmpty(paymentMethod))
                     { 
                         //Get Fee based on payment method and account number
                         fee = utility.GetFeeByPaymentMethodAndPaymentType(accountNumber, paymentMethod, "Recurring");

                         return Request.CreateResponse(HttpStatusCode.OK, new AutoPayAgreementEntity { Fee = fee, PaymentMethod = paymentMethod});
                     }
                     else
                     {
                         return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Invalid payment method.");
                     }
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in GetAutoPayPaymentMethod.", ex);
                 return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
             }
         }

         [HttpPost]
         [Route("EnrollToAutoPay")]
         public HttpResponseMessage UpdateConfirmationNumber(AutoPayAgreementEntity autoPayAgreementEntity)
         {
             try
             {
                 if (autoPayAgreementEntity != null)
                 {
                     if (string.IsNullOrWhiteSpace(autoPayAgreementEntity.AccountNumber))
                     {
                         return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Account Number cannot be blank.");
                     }
                     else
                     {
                         //Generate confirmation number
                         string confirmationNumber = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                         autoPayAgreementEntity.ConfirmationNumber = confirmationNumber;

                         string result = autoPayAgreement.UpdateAutoPayConfirmationNumber(autoPayAgreementEntity.AccountNumber, autoPayAgreementEntity.ConfirmationNumber);

                         //Send text message
                         string textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.AutoPayAgreement, autoPayAgreementEntity.AccountNumber, confirmationNumber);
                         var response = Helper.Instance.SendTextMessage(autoPayAgreementEntity.AccountNumber, textMessage);

                         return Request.CreateResponse(HttpStatusCode.OK, autoPayAgreementEntity);
                     }
                 }
                 else
                     return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Invalid account number.");
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in UpdateConfirmationNumber.", ex);
                 return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
             }
         }
    }
}
