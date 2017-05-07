using CyberSource.CyberService;
using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using MAF.SolutionByText;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace MAF.WEBAPI.Controllers
{
    [Authorize]
    // create web api AccountInformation
    [RoutePrefix("api/Payonline")]
    public class PayonlineController : ApiController
    {
        private readonly IPayOnline IPayonline;
        private readonly IAchPayment achPayment;
        private readonly ICardPayment cardPayment;

        PayonlineEntity payonlineEntity = null;
        RoutingNumberEntity routingNumberEntity = null;

        public PayonlineController(IPayOnline iPayonline, IAchPayment achPayment, ICardPayment cardPayment)
        {
            // Create object Payonline(Account Information)
            IPayonline = iPayonline;
            this.achPayment = achPayment;
            this.cardPayment = cardPayment;
        }

        ///<summary>
        /// This private datamember is used to create object of Cybersource
        /// </summary>
        private CyberService cyberInfo;

        /// <summary>
        /// This property is used to create object of CyberInfo class and check the object is already created or not
        /// </summary>
        public CyberService CyberInfo
        {
            get
            {
                if (cyberInfo == null)
                {
                    cyberInfo = new CyberService();
                }
                return cyberInfo;
            }
        }        

        #region Account Information Get
        //Get the Account Information  function
        [HttpGet]
         [Route("GET/{AccountNumber:int}")]
        public IHttpActionResult GET(int AccountNumber)
        {
            try
            {
                //  This private datamember is used to create object of Account Information
                payonlineEntity = new PayonlineEntity();


                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);

                }
                else
                {
                    // search account Information get
                    payonlineEntity = IPayonline.GetAccountInformation(Convert.ToString(AccountNumber));
                    payonlineEntity.MinimumAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                    payonlineEntity.MaximumAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]); 
                    //send web api  account Information
                    return Ok(payonlineEntity);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller Get Details Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Get RoutingNumber
        //  find Get Routing Number
        [HttpPost]
         [Route("post")]
         public IHttpActionResult post(AccountInformationEntity accountInformation)
         {
             try
             {
                 routingNumberEntity = new RoutingNumberEntity();
                 if (accountInformation == null)
                 {
                     string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return BadRequest(message);

                 }
                 if (string.IsNullOrWhiteSpace(accountInformation.AccountNumber) || string.IsNullOrWhiteSpace(accountInformation.CheckingAccountNumber) || string.IsNullOrWhiteSpace(accountInformation.Amount) || string.IsNullOrWhiteSpace(accountInformation.Routing))
                 {
                     string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return BadRequest(message);
                 }
                 else
                 {
                     payonlineEntity = IPayonline.GetAccountInformation(Convert.ToString(accountInformation.AccountNumber));
                     if (Convert.ToDecimal(accountInformation.Amount) < Convert.ToDecimal(payonlineEntity.AmountPastDue.Replace("$","")) && payonlineEntity.AcctDaysPastDue > 0)
                         routingNumberEntity.Error = "Minimum amount allowed: " + payonlineEntity.AmountPastDue;    
                     //Pass the parameter value into the routing number verify
                    else routingNumberEntity = achPayment.ValidatePayment(accountInformation);
                     //send web api into Account Information
                     return Ok(routingNumberEntity);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get Routing Number Action Method", ex);
                 return null;
             }
         }
         #endregion
         
        #region PaymentHistory
        //Get the Payment History  function  
         [HttpGet]
         [Route("PaymentHistory/{AccountNumber:int}")]
         public IHttpActionResult PaymentHistory(int AccountNumber)
         {
             try
             {
                 // This property is used to create object of Payment History and  list of Payment History model
                 var allOldPayment = new List<PaymentHistoryEntity>();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     // Get the Payment History
                     allOldPayment = IPayonline.GetPaymentHistory(Convert.ToString(AccountNumber));
                     // send Payment History into web api
                     return Ok(allOldPayment);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get Payment History Action Method", ex);
                 return null;
             }
         }
        #endregion
         //Get the Last Payment History  function 
         #region LastPaymentHistory
         [HttpGet]
         [Route("LastpaymentHistory/{AccountNumber:int}")]
         public IHttpActionResult LastpaymentHistory(int AccountNumber)
         {
             try
             {
                 // This property is used to create object of Last Payment History .
                 var lastPyament = new PaymentHistoryEntity();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     lastPyament = IPayonline.LastPaymentHistory(Convert.ToString(AccountNumber));
                     // send last  Payment  into web api
                     return Ok(lastPyament);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Last Payment History Action Method", ex);
                 return null;
             }
         }

           #endregion

         #region Search Payment
         //Search Payment  function 
         [HttpPost]
         [Route("PaymentSearch")]
         public IHttpActionResult PaymentSearch(SearchPaymentEntity searchPayment)
         {
             try
             {
                 var Payment = new List<PaymentHistoryEntity>();

                 if (string.IsNullOrWhiteSpace(searchPayment.AccountNumber) || string.IsNullOrWhiteSpace(searchPayment.FromDate) || string.IsNullOrWhiteSpace(searchPayment.ToDate))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     // Search and send Payment History into web api
                     Payment = IPayonline.SearchPaymentHistory(searchPayment);

                     return Ok(Payment);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Payment Search Action Method", ex);
                 return null;
             }
         }
         #endregion


         #region GetSearchPaymentDetails
         //Search Payment  function 
         [HttpGet]
         [Route("GetSearchPaymentDetails/{AccountNumber:int}")]
         public IHttpActionResult GetSearchPaymentDetails(int AccountNumber)
         {
             try
             {
                 string accountNumber = Convert.ToString(AccountNumber);
                 var Payment = new List<SearchPaymentDetailsEntity>();
                 SearchPaymentEntity search = new SearchPaymentEntity();
                 if (string.IsNullOrWhiteSpace(accountNumber))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     // Search and send Payment History into web api
                     Payment = IPayonline.SearchPaymentDetails(accountNumber, search);

                     return Ok(Payment);
                 }
             }
             catch (Exception ex)
             {
               MAF.BAL. Logger.WriteErrorLog("Error in PayOnline Controller in Get SearchPayment Details Get Action Method", ex);
                 return null;
             }
         }

         [HttpPost]
         [Route("CancelSchedulePayment")]
         public IHttpActionResult CancelSchedulePayment(SearchPaymentDetailsEntity searchPaymentDetails)
         {
             string textMessage = string.Empty;
             try
             {
                 if (string.IsNullOrEmpty(Convert.ToString(searchPaymentDetails.Id)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     
                     IPayonline.CancelSchedulePayment(searchPaymentDetails.Id);
                     textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.CancelSchedule, searchPaymentDetails.ScheduledAmount, searchPaymentDetails.AccountNumber, String.Format("{0:MM/dd/yyyy}", searchPaymentDetails.ScheduleDate));
                     var response = Helper.Instance.SendTextMessage(searchPaymentDetails.AccountNumber, textMessage);
                     return Ok();
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Cancel Scheduel Payment Post Action Method", ex);
                 return null;
             }
         }
         #endregion

         #region Post Search Payment Schedule
         //Search Payment  function 
         [HttpPost]
         [Route("SearchPaymentDetails")]
         public IHttpActionResult SearchPaymentDetails(SearchPaymentEntity searchPayment)
         {
             try
             {
                 string accountNumber = null;
                 var Payment = new List<SearchPaymentDetailsEntity>();
                 if (string.IsNullOrWhiteSpace(searchPayment.AccountNumber))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     // Search and send Payment History into web api
                     Payment = IPayonline.SearchPaymentDetails(accountNumber, searchPayment);

                     return Ok(Payment);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get SearchPayment Details Post Action Method", ex);
                 return null;
             }
         }
         #endregion
        
         #region Card Details



         //Check Valid Card Transaction 
         [HttpPost]
         [Route("CheckValidCardTransaction")]
         public IHttpActionResult CheckValidCardTransaction(AccountInformationEntity accountInfo)
         {
             try
             {
                 payonlineEntity = new PayonlineEntity();
                 var CheckCardMaxTransaction = string.Empty;
                 if (string.IsNullOrWhiteSpace(Convert.ToString(accountInfo.AccountNumber)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     payonlineEntity = IPayonline.GetAccountInformation(Convert.ToString(accountInfo.AccountNumber));
                     if (Convert.ToDecimal(accountInfo.Amount) < Convert.ToDecimal(payonlineEntity.AmountPastDue.Replace("$", "")) && payonlineEntity.AcctDaysPastDue > 0)
                         CheckCardMaxTransaction = "Minimum amount allowed: " + payonlineEntity.AmountPastDue;
                     else CheckCardMaxTransaction = cardPayment.CheckValidCardTransaction(accountInfo.AccountNumber, accountInfo.Amount, accountInfo.FuturePaymentDate, accountInfo.IsTotalAmountDue);
                     return Ok(CheckCardMaxTransaction);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Check Max Card Limit Action Method", ex);
                 return null;
             }
         }

    

         //Get the Card Details function  
         [HttpGet]
         [Route("CardDetails/{AccountNumber:int}")]
         public IHttpActionResult CardDetails(int AccountNumber)
         {
             try
             {
                 var cardDetailsList = new List<CardDetailsEntity>();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {

                     cardDetailsList = cardPayment.CardDetails(Convert.ToString(AccountNumber));
                     return Ok(cardDetailsList);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get Card Details Action Method", ex);
                 return null;
             }
         }

         [HttpPost]
         [Route("DeleteCard/{tokeinId:long}")]
         public IHttpActionResult DeleteCard(long tokeinId)
         {
             try
             {
                 var cardDetailsList = new List<CardDetailsEntity>();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(tokeinId)))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     cardPayment.DeleteCard(Convert.ToString(tokeinId));                     
                     return Ok("Success");
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Delete Card Details Action Method", ex);
                 return null;
             }
         }

         //Get the Billing Address function  
         [HttpGet]
         [Route("BillingAddress/{accountNumber:int}/{tokenId:long}")]
         public HttpResponseMessage BillingAddress(int accountNumber, long tokenId)
         {
             try
             {
                 var BillingAddress = new BillingAddressEntity();

                 if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                 {
                     // Error message send
                     return Request.CreateErrorResponse(HttpStatusCode.BadRequest, MAF.BAL.ResourceFile.Common.RequestInvalid);
                 }
                 else
                 {

                     BillingAddress = cardPayment.GetBillingAddress(Convert.ToString(accountNumber), Convert.ToString(tokenId));
                     return Request.CreateResponse(HttpStatusCode.OK, BillingAddress);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in Future Pay Controller in Get Billing Address Action Method", ex);
                 return null;
             }
         }
         #endregion

         #region AchDetails

         /// <summary>
         /// Get saved ach details
         /// </summary>
         /// <param name="AccountNumber"></param>
         /// <returns></returns>
         [HttpGet]
         [Route("GetSavedAch/{accountNumber:int}")]
         public HttpResponseMessage GetSavedAchDetails(int accountNumber)
         {
             try
             {
                 var achDetails = new List<AchDetailEntity>();
                 string message = string.Empty;
                 if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                 {
                     message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                 }
                 else
                 {
                     // search account Information get
                     achDetails = achPayment.GetSavedAchDetail(Convert.ToString(accountNumber));
                     //send web api  account Information
                     return Request.CreateResponse(HttpStatusCode.OK, achDetails);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller Get Saved Ach Details Action Method", ex);
                 return null;
             }
         }

         /// <summary>
         ///  Action method to Delete Ach detail
         /// </summary>
         /// <param name="bankAccountNumber">Bank Account Number</param>
         /// <param name="accountNumber">Account Number</param>
         /// <returns></returns>
         [HttpDelete]
         [Route("DeleteSavedAch/{ID:long}/{accountNumber:int}")]
         public HttpResponseMessage DeleteSavedAch(long ID, int accountNumber)
         {
             try
             {
                 string message = string.Empty;

                 if (string.IsNullOrWhiteSpace(Convert.ToString(ID)) || string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                 {
                     message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);

                 }
                 else
                 {
                     message = achPayment.DeleteSavedAchDetail(ID, accountNumber);
                     //send web api  account Information
                     return Request.CreateResponse(HttpStatusCode.OK, message);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in AutoPay Controller in Delete Ach Amount Action Method", ex);
                 return null;
             }
         }

        #endregion


         #region FutureDate Validation
         //  
         [HttpPost]
         [Route("FutureDateValidation")]
         public IHttpActionResult FutureDateValidation(AccountInformationEntity accountInformation)
         {
             try
             {
                 var futureDateValidation = new RoutingNumberEntity();

                 if (!string.IsNullOrWhiteSpace(accountInformation.FuturePaymentDate))
                 {
                     DateTime FutureDate = Convert.ToDateTime(accountInformation.FuturePaymentDate);
                     if (DateTime.Now.Date >= FutureDate.Date)
                     {
                         futureDateValidation.ErrorFutureDate = "Date must be greater than today date.";
                         return Ok(futureDateValidation);
                     }
                 }

                 return Ok(futureDateValidation);
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Future Date Validation Action Method", ex);
                 return null;
             }
         }
         #endregion


         #region Check NSF Status

         /// <summary>
         /// Check NSF Status
         /// </summary>
         /// <param name="AccountNumber"></param>
         /// <returns></returns>
         [HttpGet]
         [Route("CheckNSFStatus/{accountNumber:int}")]
         public HttpResponseMessage CheckNSFStatus(int accountNumber)
         {
             try
             {
                 string message = string.Empty;
                 if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                 {
                     message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                 }
                 else
                 {                     
                     var NSFStatus = IPayonline.CheckNSFStatus(Convert.ToString(accountNumber));
                     return Request.CreateResponse(HttpStatusCode.OK, NSFStatus);
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller Check NSF Status Action Method", ex);
                 return null;
             }
         }
#endregion

        #region Save Card Address
         string status, message, statusCode = string.Empty;
         //Save Card Address function
         [HttpPost]
         [Route("SaveCardAddress")]
         public IHttpActionResult SaveCardAddress(CardInfoEntity cardInfo)
         {
             
             try
             {
                 var cardDetailsList = new List<CardDetailsEntity>();

                 if (string.IsNullOrWhiteSpace(cardInfo.AccountNumber) || string.IsNullOrWhiteSpace(cardInfo.FirstName) || string.IsNullOrWhiteSpace(cardInfo.LastName) || string.IsNullOrWhiteSpace(cardInfo.Street) || string.IsNullOrWhiteSpace(cardInfo.City)
                      || string.IsNullOrWhiteSpace(cardInfo.State) || string.IsNullOrWhiteSpace(cardInfo.PostelCode) || string.IsNullOrWhiteSpace(cardInfo.Email) || string.IsNullOrWhiteSpace(cardInfo.TokenId) ||
                      string.IsNullOrWhiteSpace(cardInfo.CardNumber) || string.IsNullOrWhiteSpace(cardInfo.ExpirationMonth) || string.IsNullOrWhiteSpace(cardInfo.ExpirationYear) ||
                      string.IsNullOrWhiteSpace(cardInfo.CardType) || string.IsNullOrWhiteSpace(cardInfo.CardName))
                 {
                     // Error message send
                     return NotFound();
                 }
                 else
                 {
                     cardInfo.MerchantReferenceCode = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                     cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); //HttpContext.Current.Request.UserHostAddress;
                     status = CyberInfo.UpdateSubscription(cardInfo);
                     if (status.Substring(0, 3) == "100")
                     {
                         cardPayment.SaveCardBillingAddress(cardInfo);
                     }
                     message = ReturnMessageSavedCardAddress(status, cardInfo, MAF.BAL.ResourceFile.Common.SaveCardAddress);
                     return Ok(message);
                 }

             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Save Card Address Action Method", ex);
                 return null;
             }
         }

         private string ReturnMessageSavedCardAddress(string status, CardInfoEntity cardInfoEntity, string Description)
         {
             CardDetailsEntity cardInfo = new CardDetailsEntity();
             if (!string.IsNullOrWhiteSpace(status))
             {
                 statusCode = status.Substring(0, 3);
                 cardInfo.AcctNo = cardInfoEntity.AccountNumber;
                 cardInfo.ReasonCode = statusCode;
                 cardInfo.TokenId = cardInfoEntity.TokenId;
                 cardInfo.CardName = cardInfoEntity.CardName;
                 cardInfo.CardNumber = cardInfoEntity.CardNumber;
                 cardInfo.CardExpiry = cardInfoEntity.ExpirationMonth + "/" + cardInfoEntity.ExpirationYear;
                 cardInfo.CardType = cardInfoEntity.CardType;
                 if (statusCode == "100")
                 {
                     cardInfo.ConfirmationId = cardInfoEntity.MerchantReferenceCode;
                     cardInfo.Description = Description;
                     cardInfo.RequestId = status.Substring(4, 22);
                     cardPayment.SaveCardTransactionLog(cardInfo);
                     return cardInfoEntity.MerchantReferenceCode;
                 }
                 else
                 {
                     cardInfo.ConfirmationId = string.Empty;
                     cardInfo.RequestId = status.Substring(4, 22);
                     cardInfo.Description = status.Remove(0, 27);
                     cardPayment.SaveCardTransactionLog(cardInfo);
                     return status.Remove(0, 27);

                 }
             }
             return string.Empty;
         }
        #endregion

    }
}