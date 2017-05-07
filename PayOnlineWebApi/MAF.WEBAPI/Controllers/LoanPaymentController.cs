using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/LoanPayment")]
    public class LoanPaymentController : ApiController
    {
        // Get interface Loan Payment
         private readonly ILoanPayment ILoanPayment;

         public LoanPaymentController(ILoanPayment iLoanPayment)
         {
             // Create object Loan Payment
            ILoanPayment = iLoanPayment;
           
        }
         //Get the loan payment search function
         #region Get Search LoanPayment
         [HttpGet]
         [Route("GetLoanPayment/{AccountNumber:int}")]
         public IHttpActionResult GetLoanPayment(int AccountNumber)
         {
             try
             {
                 //  This property datamember is used to create object of Loan Payment
                 var LoanPayment = new LoanPaymentEntity();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                 {
                     string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);


                     // Error message send
                     return BadRequest(message);
                 }
                 else
                 {
                     //Pass the parameter account number into the Loan Payment
                     LoanPayment = ILoanPayment.GetLoanPayment(Convert.ToString(AccountNumber));
                     //send web api Loan Payment Conformation
                     return Ok(LoanPayment);
                 }
             }
             catch (Exception ex)
             {
                 Logger.WriteErrorLog("Error in Loan Payment Controller Get Loan Amount Action Method", ex);
                 return null;
             }
         }

         #endregion

         //Get the Duplicate Payment function
         #region Get Duplicate Payment
         [HttpGet]
         [Route("DuplicatePayment/{AccountNumber:int}")]
         public IHttpActionResult DuplicatePayment(int AccountNumber)
         {

             try
             {
                 var objDuplicateList = new List<DuplicatePaymentEntity>();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                 {
                     return BadRequest(string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid));
                 }
                 else
                 {
                     //Pass the parameter account number into the Duplicate Payment
                     objDuplicateList = ILoanPayment.GetDuplicatePayment(Convert.ToString(AccountNumber));
                     //send web api Duplicate Payment Conformation
                     return Ok(objDuplicateList);
                 }
             }
             catch (Exception ex)
             {
                 Logger.WriteErrorLog("Error in Duplicate Payment Controller Get Duplicate Amount Action Method", ex);
                 return null;
             }
         }

         #endregion
        /// <summary>
        /// Action method to return alerts
        /// </summary>
        /// <param name="AccountNumber"></param>
        /// <returns></returns>
         [HttpGet]
         [Route("GetAlerts/{AccountNumber}")]
         public HttpResponseMessage Alerts(string AccountNumber)
         {

             try
             {
                 if (string.IsNullOrWhiteSpace(AccountNumber))
                 {
                     return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                 }
                 else
                 {
                     var alerts = ILoanPayment.GetAlerts(AccountNumber);

                     return Request.CreateResponse(HttpStatusCode.OK, alerts);
                 }
             }
             catch (Exception ex)
             {
                 Logger.WriteErrorLog("Error in Get Alerts Action Method", ex);
                 return null;
             }
         }

    }
}
