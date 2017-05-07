using MAF.BAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
     [Authorize]
    // create web api AccountInformation
    [RoutePrefix("api/PaymentSchedule")]
    public class PaymentScheduleController : ApiController
    {

        #region Search Payment
        //Search Payment  function 
        [HttpPost]
        [Route("PaymentScheduleSearch")]
        public IHttpActionResult PaymentSearch(SearchPaymentEntity searchPayment)
        {
            try
            {
                var paymentSchedule = new List<PaymentSchedule>();

                if (string.IsNullOrWhiteSpace(searchPayment.AccountNumber) || string.IsNullOrWhiteSpace(searchPayment.FromDate) || string.IsNullOrWhiteSpace(searchPayment.ToDate))
                {
                    // Error message send
                    return NotFound();
                }
                else
                {
                    // Search and send Payment History into web api
                    //Payment = IPayonline.paymentHistorySearch(searchPayment);

                    return Ok(paymentSchedule);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in PaymentSchedule Controller in Payment Schedule Search Action Method", ex);
                return null;
            }
        }
        #endregion
    }
}
