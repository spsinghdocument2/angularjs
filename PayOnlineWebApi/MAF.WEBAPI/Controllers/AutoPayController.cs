using CyberSource.CyberService;
using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/AutoPay")]
    public class AutoPayController : ApiController
    {
        private readonly IAutoPay IAutoPay;
        private readonly IUtility utility;
        private readonly ICardPayment cardPayment;
        private readonly IAchPayment achPayment;

        public AutoPayController(IAutoPay iAutoPay, IUtility utility, ICardPayment cardPayment, IAchPayment achPayment)
        {
            IAutoPay = iAutoPay;
            this.utility = utility;
            this.cardPayment = cardPayment;
            this.achPayment = achPayment;
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

        #region GET Ach Detail
        /// <summary>
        /// Action method to GET Ach Detail
        /// </summary>
        /// <param name="AccountNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GETAchDetail/{AccountNumber:int}")]
        public HttpResponseMessage GetAchDetail(int AccountNumber)
        {
            try
            {

                var AchDetail = new List<AchDetailEntity>();
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    // search account Information get
                    AchDetail = IAutoPay.GetAchDetail(Convert.ToString(AccountNumber));
                    //send web api  account Information
                    return Request.CreateResponse(HttpStatusCode.OK, AchDetail);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Autopay Controller Get Details Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Save Ach Auto Pay
        /// <summary>
        ///  Action method to Save Ach Amount Auto Pay
        /// </summary>
        /// <param name="PaymentConfirmationEntity">PaymentConfirmationEntity</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAchAmount")]
        public HttpResponseMessage SaveAchAmount(PaymentConfirmationEntity collection)
        {
            try
            {
                string textMessage = string.Empty;
                string message = string.Empty;
                if (collection == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                if (string.IsNullOrWhiteSpace(collection.AccountNumber) || string.IsNullOrWhiteSpace(collection.PaymentAmount) || string.IsNullOrWhiteSpace(collection.FeeAmoun) || string.IsNullOrWhiteSpace(collection.RountingNumber) ||
                   string.IsNullOrWhiteSpace(collection.BankAccountNumber) || string.IsNullOrWhiteSpace(collection.BankName) || string.IsNullOrWhiteSpace(collection.BankHolder) || string.IsNullOrWhiteSpace(collection.AccountType) ||
                   string.IsNullOrWhiteSpace(collection.UpdatedPhone) || string.IsNullOrWhiteSpace(collection.UpdatedPhoneDate) || string.IsNullOrWhiteSpace(collection.Email) || string.IsNullOrWhiteSpace(collection.SaveAccountFuture) || string.IsNullOrWhiteSpace(collection.BankAccountName))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    //Pass the parameter AutoPay models into the Confirmation
                    message = IAutoPay.SaveAchAutoPaySetup(collection);


                    if (message.Length == 10)
                    {
                        string bankNumber = collection.BankAccountNumber;
                        bankNumber = bankNumber.Substring(bankNumber.Length - 4);
                        // send text message auto pay 
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.AutoPayEnrollment, collection.AccountNumber, bankNumber, message);
                        var response = Helper.Instance.SendTextMessage(collection.AccountNumber, textMessage);
                    }


                    //send web api  account Information
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in AutoPay Controller in SaveAchAmount Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Get AutoPay Recurring
        /// <summary>
        /// Action method to Get Auto Pay Recurring
        /// </summary>
        /// <param name="AccountNumber">AccountNumber</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAutoPayRecurring/{AccountNumber:int}")]
        public HttpResponseMessage GetAutoPayRecurring(int AccountNumber)
        {
            try
            {
                var AutoPayDetail = new List<AutoPayRecurringEntity>();
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    // Get Auto Pay Recurring
                    AutoPayDetail = IAutoPay.GetAutoPayDetail(Convert.ToString(AccountNumber));
                    //send web api  Auto Pay
                    return Request.CreateResponse(HttpStatusCode.OK, AutoPayDetail);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Autopay Controller Get Details Action Method", ex);
                return null;
            }
        }

        #endregion

        #region delete AutoPay Recurring
        /// <summary>
        /// Action method to delete Auto Pay Recurring
        /// </summary>
        /// <param name="AccountNumber">AccountNumber</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAutoPayRecurring/{AccountNumber:int}")]
        public HttpResponseMessage DeleteAutoPayRecurring(int AccountNumber)
        {
            string textMessage = string.Empty;
            try
            {
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    // Delete Auto Pay Recurring
                    message = IAutoPay.DeleteAutoPay(Convert.ToString(AccountNumber));

                    // send text message auto pay 
                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.DeleteAutoPay, Convert.ToString(AccountNumber));
                    var response = Helper.Instance.SendTextMessage(Convert.ToString(AccountNumber), textMessage);
                    //send web api  Auto Pay
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Autopay Controller Delete Details Action Method", ex);
                return null;
            }
        }
        #endregion

        string tokenId = string.Empty;//Use Global
        [HttpPost]
        [Route("CardAutoPaySchedule")]
        public HttpResponseMessage CardAutoPaySchedule(CardInfoEntity cardInfo)
        {
            try
            {
                string status = string.Empty;
                string message = string.Empty;
                string textMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(cardInfo.AccountNumber) || string.IsNullOrWhiteSpace(cardInfo.FirstName) || string.IsNullOrWhiteSpace(cardInfo.LastName) || string.IsNullOrWhiteSpace(cardInfo.Street) || string.IsNullOrWhiteSpace(cardInfo.City)
                 || string.IsNullOrWhiteSpace(cardInfo.State) || string.IsNullOrWhiteSpace(cardInfo.PostelCode) || string.IsNullOrWhiteSpace(cardInfo.Email) ||
                 string.IsNullOrWhiteSpace(cardInfo.CardNumber) || string.IsNullOrWhiteSpace(cardInfo.ExpirationMonth) || string.IsNullOrWhiteSpace(cardInfo.ExpirationYear) ||
              string.IsNullOrWhiteSpace(cardInfo.CardType) || string.IsNullOrWhiteSpace(cardInfo.CvNumber) || string.IsNullOrWhiteSpace(cardInfo.Amount) || string.IsNullOrWhiteSpace(cardInfo.CvNumber))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    var cardLimit = cardPayment.CheckCardMaxLimit(cardInfo);
                    if (cardLimit != "Blocked")
                    {
                        //send web api  Auto Pay
                        cardInfo.MerchantReferenceCode = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                        cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); // HttpContext.Current.Request.UserHostAddress;
                        cardInfo.CardType = cardInfo.CardType == "Visa" ? "001" : "002";
                        status = CyberInfo.TokenizationSubscription(cardInfo);
                        message = ReturnMessageCardPayment(status, cardInfo, MAF.BAL.ResourceFile.Common.CardSubscription);
                        cardInfo.TokenId = tokenId;
                        if (status.Substring(0, 3) == "100")
                        {
                            IAutoPay.InsertAutoPayCardSchedule(cardInfo);
                            IAutoPay.SaveCardBillingAddress(cardInfo);
                            message = "successfull";

                            // send text message auto pay 
                            string CardNumber = cardInfo.CardNumber;
                            CardNumber = CardNumber.Substring(CardNumber.Length - 4);
                            textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.AutoPayEnrollment, cardInfo.AccountNumber, CardNumber, cardInfo.MerchantReferenceCode);
                            var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else return Request.CreateResponse(HttpStatusCode.OK, "Your card has been blocked for this merchant for a day. Please try another card.");
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in AutoPay Card Payment Action Method", ex);
                return null;
            }
        }

        [HttpPost]
        [Route("SaveCardAutoPaySchedule")]
        public HttpResponseMessage SaveCardAutoPaySchedule(CardInfoEntity cardInfo)
        {
            try
            {
                string message = string.Empty;
                string textMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(cardInfo.AccountNumber) || string.IsNullOrWhiteSpace(cardInfo.FirstName) || string.IsNullOrWhiteSpace(cardInfo.LastName) || string.IsNullOrWhiteSpace(cardInfo.Street) || string.IsNullOrWhiteSpace(cardInfo.City)
                 || string.IsNullOrWhiteSpace(cardInfo.State) || string.IsNullOrWhiteSpace(cardInfo.PostelCode) || string.IsNullOrWhiteSpace(cardInfo.Email) ||
                 string.IsNullOrWhiteSpace(cardInfo.CardNumber) || string.IsNullOrWhiteSpace(cardInfo.ExpirationMonth) || string.IsNullOrWhiteSpace(cardInfo.ExpirationYear) ||
              string.IsNullOrWhiteSpace(cardInfo.CardType) || string.IsNullOrWhiteSpace(cardInfo.CvNumber) || string.IsNullOrWhiteSpace(cardInfo.Amount) || string.IsNullOrWhiteSpace(cardInfo.CvNumber))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    var cardLimit = cardPayment.CheckCardMaxLimit(cardInfo);
                    if (cardLimit != "Blocked")
                    {
                        //send web api  Auto Pay
                        cardInfo.MerchantReferenceCode = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                        cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); // HttpContext.Current.Request.UserHostAddress;
                        cardInfo.CardType = cardInfo.CardType == "Visa" ? "001" : "002";
                        message = cardInfo.MerchantReferenceCode;
                        IAutoPay.InsertAutoPayCardSchedule(cardInfo);
                        IAutoPay.SaveCardBillingAddress(cardInfo);
                        message = "successfull";
                        // send text message auto pay 
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.AutoPayEnrollment, cardInfo.AccountNumber, cardInfo.CardNumber, cardInfo.MerchantReferenceCode);
                        var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);

                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else return Request.CreateResponse(HttpStatusCode.OK, "Your card has been blocked for this merchant for a day. Please try another card.");
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in AutoPay Card Payment Action Method", ex);
                return null;
            }
        }

        private string ReturnMessageCardPayment(string status, CardInfoEntity cardInfoEntity, string Description)
        {
            string statusCode = string.Empty;
            string textMessage = string.Empty;
            CardDetailsEntity cardInfo = new CardDetailsEntity();
            if (!string.IsNullOrWhiteSpace(status))
            {
                statusCode = status.Substring(0, 3);
                cardInfo.AcctNo = cardInfoEntity.AccountNumber;
                cardInfo.ReasonCode = statusCode;
                cardInfo.CardName = cardInfoEntity.CardName;
                cardInfo.CardNumber = cardInfoEntity.CardNumber.Substring(cardInfoEntity.CardNumber.Length - 4);
                cardInfo.CardExpiry = cardInfoEntity.ExpirationMonth + "/" + cardInfoEntity.ExpirationYear;
                cardInfo.CardType = cardInfoEntity.CardType;
                if (statusCode == "100")
                {
                    tokenId = status.Substring(status.Length - 16);
                    cardInfo.TokenId = tokenId;
                    cardInfo.ConfirmationId = cardInfoEntity.MerchantReferenceCode;
                    cardInfo.Description = Description;
                    cardInfo.RequestId = status.Substring(4, 22);
                    IAutoPay.SaveCardTransactionLog(cardInfo);
                    return cardInfoEntity.MerchantReferenceCode;
                }
                else
                {
                    cardInfo.ConfirmationId = string.Empty;
                    cardInfo.TokenId = string.Empty;
                    cardInfo.RequestId = status.Substring(4, 22);
                    cardInfo.Description = status.Remove(0, 27);
                    IAutoPay.SaveCardTransactionLog(cardInfo);
                    return status.Remove(0, 27);

                }
            }
            return string.Empty;
        }

        #region Ach Fee
        /// <summary>
        ///  Action method to Ach fee in Auto Pay
        /// </summary>
        /// <param name="accountNumber">accountNumber</param>
        /// <returns>fee</returns>
        [HttpGet]
        [Route("GetAchFeeAutopay/{AccountNumber:int}")]
        public IHttpActionResult GetAchFeeAutopay(int AccountNumber)
        {
            try
            {
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);
                }
                else
                {
                    ////Pass the parameter account number into the get Fee 
                    //message = IAutoPay.AchFee(Convert.ToString(AccountNumber));

                    //Get Fee based on payment method and account number
                    message = utility.GetFeeByPaymentMethodAndPaymentType(Convert.ToString(AccountNumber), "ACH", "Recurring");
                    //send web api get Fee
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in autopay Controller Get ach fee Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Card Fee
        /// <summary>
        ///  Action method to card fee in Auto Pay
        /// </summary>
        /// <param name="accountNumber">accountNumber</param>
        /// <returns>fee</returns>
        [HttpGet]
        [Route("GetCardFeeAutopay/{AccountNumber:int}")]
        public IHttpActionResult GetCardFeeAutopay(int AccountNumber)
        {
            try
            {
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    // Error message send
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);
                }
                else
                {
                    ////Pass the parameter account number into the get Fee 
                    //message = IAutoPay.CardFee(Convert.ToString(AccountNumber));

                    //Get Fee based on payment method and account number
                    message = utility.GetFeeByPaymentMethodAndPaymentType(Convert.ToString(AccountNumber), "Card", "Recurring");

                    //send web api get Fee
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in autopay Controller Get card fee Action Method", ex);
                return null;
            }
        }
        #endregion

        //Get the Duplicate Payment function
        #region Get Duplicate Payment
        [HttpGet]
        [Route("AutoPayDuplicatePayment/{AccountNumber:int}")]
        public IHttpActionResult AutoPayDuplicatePayment(int AccountNumber)
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
                    objDuplicateList = IAutoPay.GetAutoPayDuplicatePayment(Convert.ToString(AccountNumber));
                    //send web api Duplicate Payment Conformation
                    return Ok(objDuplicateList);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Auto Pay Agreement Controller Get Auto Pay Duplicate Payment Action Method", ex);
                return null;
            }
        }

        #endregion

        #region ValidateTransaction
        // validate transaction for ach
        [HttpPost]
        [Route("ValidateTransaction")]
        public IHttpActionResult ValidateTransaction(AccountInformationEntity accountInformation)
        {
            try
            {
                string objValidateTransaction = string.Empty;
                RoutingNumberEntity routingNumberEntity = new RoutingNumberEntity();
                Utility objUtil = new Utility();
                if (accountInformation == null)
                {
                    string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);
                }

                if (string.IsNullOrWhiteSpace(accountInformation.AccountNumber) || string.IsNullOrWhiteSpace(accountInformation.Amount))
                {
                    string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);
                }
                else
                {
                    accountInformation.IsTotalAmountDue = true;
                    if ( accountInformation.PaymentMethod == "ACH")
                    {
                        objValidateTransaction = objUtil.ValidateTransaction(accountInformation.AccountNumber, Convert.ToDecimal(accountInformation.Amount), accountInformation.FuturePaymentDate, accountInformation.IsTotalAmountDue);
                        if (objValidateTransaction != "true")
                         {
                                routingNumberEntity.Error = objValidateTransaction;
                         }
                           
                        else if (!string.IsNullOrWhiteSpace(accountInformation.Routing))
                        {
                            //Pass the parameter value into the routing number verify
                            routingNumberEntity = achPayment.ValidateRoutingNumber(accountInformation.Routing);
                        }
                    }
                   
                  
                }

                return Ok(routingNumberEntity);
            }

            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in AutoPay Controller in Get Routing Number Action Method", ex);
                return null;
            }
        }
        #endregion

    }
}
