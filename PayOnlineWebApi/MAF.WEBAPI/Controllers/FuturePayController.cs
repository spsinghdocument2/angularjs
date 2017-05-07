using CyberSource.CyberService;
using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
     [Authorize]
    // create web api AccountInformation
    [RoutePrefix("api/FuturePay")]
    public class FuturePayController : ApiController
    {
        // Get interface Account Information
        private readonly IFuturePay futurePay;
        private readonly IUtility utility;
        FuturePayEntity futurePayEntity = null;
        
        private readonly IAchPayment achPayment;
        private readonly ICardPayment cardPayment;
        public FuturePayController(IFuturePay iFuturePay, IUtility utility, IAchPayment achPayment, ICardPayment cardPayment)
        {
            futurePay = iFuturePay;
            this.utility = utility;
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

        #region Get Account Information
        //Get the Account Information  function
        [HttpGet]
        [Route("GetAccountInfo/{AccountNumber:int}/{IsFuturePayWithAgreement:bool}")]
        public IHttpActionResult GetAccountInfo(int AccountNumber, bool IsFuturePayWithAgreement)
        {
            try
            {
                futurePayEntity = new FuturePayEntity();
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    // Error message send
                    return BadRequest(string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid));
                }
                else
                {
                    futurePayEntity = futurePay.GetAccountInfo(AccountNumber);
                    if (IsFuturePayWithAgreement)
                    {
                        futurePayEntity.NextDueDate = futurePayEntity.DueDate;
                        futurePayEntity.DueDate = DateTime.Now.ToShortDateString();
                    }
                    futurePayEntity.MinimumAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                    futurePayEntity.MaximumAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]); 

                    return Ok(futurePayEntity);
                }
            }
            catch (Exception ex)
            {
              MAF.BAL.Logger.WriteErrorLog("Error in future Pay Controller Get account Details Action Method", ex);
                return null;
            }
        }
        #endregion

        #region ACH

        /// <summary>
        ///Get saved ach account details
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSavedAchDetail/{accountNumber:int}")]
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
        ///Delete ach account details
        /// </summary>
        /// <param name="ID">ID</param>
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

        [HttpPost]
        [Route("ValidateTransaction")]
        public IHttpActionResult ValidateTransaction(AccountInformationEntity accountInformation)
        {
            try
            {
                string objValidateTransaction = string.Empty;
                RoutingNumberEntity routingNumberEntity = new RoutingNumberEntity();
                FuturePayEntity futurePayEntity = new FuturePayEntity();
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
                        futurePayEntity = futurePay.GetAccountInfo(Convert.ToInt32(accountInformation.AccountNumber));
                        if (accountInformation.IsFuturePayWithAgreement)
                        {
                            futurePayEntity.NextDueDate = futurePayEntity.DueDate;
                            futurePayEntity.DueDate = DateTime.Now.ToShortDateString();
                        }
                        bool isValidFuturePayDate = ValidateFuturePayDate(Convert.ToDateTime(accountInformation.FuturePaymentDate), Convert.ToDateTime(futurePayEntity.DueDate), Convert.ToDateTime(futurePayEntity.NextDueDate));
                        if (isValidFuturePayDate && accountInformation.PaymentMethod == "ACH")
                        {
                            objValidateTransaction = objUtil.ValidateTransaction(accountInformation.AccountNumber, Convert.ToDecimal(accountInformation.Amount),accountInformation.FuturePaymentDate,accountInformation.IsTotalAmountDue);
                            if (objValidateTransaction != "true")
                            {
                                routingNumberEntity.Error = objValidateTransaction;
                            }
                            else if (Convert.ToDecimal(accountInformation.Amount) < futurePayEntity.Amount && futurePayEntity.AcctDaysPastDue > 0)
                            {
                                routingNumberEntity.Error = "Minimum amount allowed: $" + futurePayEntity.Amount;
                            }
                            else if (!string.IsNullOrWhiteSpace(accountInformation.Routing))
                            {
                                //Pass the parameter value into the routing number verify
                                routingNumberEntity = achPayment.ValidateRoutingNumber(accountInformation.Routing);
                            }
                        }
                        else if (isValidFuturePayDate && accountInformation.PaymentMethod == "Card")
                        {
                            objValidateTransaction = cardPayment.CheckValidCardTransaction(accountInformation.AccountNumber,accountInformation.Amount,accountInformation.FuturePaymentDate,accountInformation.IsTotalAmountDue);
                            if (objValidateTransaction != "true")
                            {
                                routingNumberEntity.Error = objValidateTransaction;
                            }
                            else if (Convert.ToDecimal(accountInformation.Amount) < futurePayEntity.Amount && futurePayEntity.AcctDaysPastDue > 0)
                            {
                                routingNumberEntity.Error = "Minimum amount allowed: $" + futurePayEntity.Amount;
                            }                           
                        }
                        else if (accountInformation.IsFuturePayWithAgreement) routingNumberEntity.ErrorFutureDate = "Future Pay Date must be between Today's Date and Due Date";
                        else routingNumberEntity.ErrorFutureDate = "Future Pay Date must be between Due Date and Next Due Date";
                    }

                    return Ok(routingNumberEntity);
                }
            
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get Routing Number Action Method", ex);
                return null;
            }
        }


        // This Post return the Payment Confirmation view page.
        [HttpPost]
        [Route("ACHFuturePay")]
        public IHttpActionResult ACHFuturePay(PaymentConfirmationEntity paymentConfirmation)
        {
            try
            {
                string message = string.Empty;
                string textMessage = string.Empty;
                if (paymentConfirmation == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);

                }

                if (string.IsNullOrWhiteSpace(paymentConfirmation.AccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.PaymentAmount) || string.IsNullOrWhiteSpace(paymentConfirmation.FeeAmoun) || string.IsNullOrWhiteSpace(paymentConfirmation.RountingNumber) ||
                    string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.BankName) || string.IsNullOrWhiteSpace(paymentConfirmation.BankHolder) || string.IsNullOrWhiteSpace(paymentConfirmation.AccountType) ||
                    string.IsNullOrWhiteSpace(paymentConfirmation.UpdatedPhone) || string.IsNullOrWhiteSpace(paymentConfirmation.Email) || string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountName))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);
                }
                else
                {
                    paymentConfirmation.SaveAccountFuture = "1";
                    //Pass the parameter paymentConfirmation models into the Confirmation
                    message = achPayment.SavePayment(paymentConfirmation);

                    // send text message Future pay 
                    if (message.Length == 10)
                    {
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, (Convert.ToDecimal(paymentConfirmation.PaymentAmount) + Convert.ToDecimal(paymentConfirmation.FeeAmoun)).ToString(), paymentConfirmation.AccountNumber, paymentConfirmation.FuturePaymentDate, message);
                        var response = Helper.Instance.SendTextMessage(paymentConfirmation.AccountNumber, textMessage);
                    }

                    //send web api Loan Payment
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Future Pay Controller in Ach Future Pay Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Card
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
                MAF.BAL.Logger.WriteErrorLog("Error in Future Pay Controller in Get Card Details Action Method", ex);
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
                MAF.BAL.Logger.WriteErrorLog("Error in Future Pay Controller in Delete Card Details Action Method", ex);
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


        #region Card Payment
        string status = string.Empty;
        string statusCode = string.Empty;
        string tokenId = string.Empty;
        string message = string.Empty;
        //Card Payment  function 
        [HttpPost]
        [Route("CardPayment")]
        public IHttpActionResult CardPayment(CardInfoEntity cardInfo)
        {
            try
            {
                var cardDetailsList = new List<CardDetailsEntity>();
                string textMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(cardInfo.AccountNumber) || string.IsNullOrWhiteSpace(cardInfo.FirstName) || string.IsNullOrWhiteSpace(cardInfo.LastName) || string.IsNullOrWhiteSpace(cardInfo.Street) || string.IsNullOrWhiteSpace(cardInfo.City)
                    || string.IsNullOrWhiteSpace(cardInfo.State) || string.IsNullOrWhiteSpace(cardInfo.PostelCode) || string.IsNullOrWhiteSpace(cardInfo.Email) ||
                    string.IsNullOrWhiteSpace(cardInfo.CardNumber) || string.IsNullOrWhiteSpace(cardInfo.ExpirationMonth) || string.IsNullOrWhiteSpace(cardInfo.ExpirationYear) ||
                    string.IsNullOrWhiteSpace(cardInfo.CardType) || string.IsNullOrWhiteSpace(cardInfo.CvNumber) || string.IsNullOrWhiteSpace(cardInfo.Amount) || string.IsNullOrWhiteSpace(cardInfo.CvNumber) || string.IsNullOrWhiteSpace(cardInfo.CardName))
                {
                    // Error message send
                    return NotFound();
                }
                else
                {
                    var cardLimit = cardPayment.CheckCardMaxLimit(cardInfo);
                    if (cardLimit != "Blocked")
                    {
                        cardInfo.MerchantReferenceCode = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                        cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); // HttpContext.Current.Request.UserHostAddress;

                        var checkCardType = string.Empty;
                        checkCardType = cardInfo.CardType;
                        cardDetailsList = cardPayment.CardDetails(cardInfo.AccountNumber);
                        cardInfo.Amount = (Convert.ToDecimal(cardInfo.Amount) + Convert.ToDecimal(cardInfo.Fee)).ToString();
                        var CardDetails = (from p in cardDetailsList where (p.CardNumber == cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4) && p.CardType == checkCardType && p.CardExpiry == cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear) select p).FirstOrDefault();
                        cardInfo.CardType = checkCardType == "V" ? "001" : "002";
                        if (!string.IsNullOrEmpty(cardInfo.TokenId))
                        {
                            cardPayment.SaveCardInfo(cardInfo);
                            cardPayment.SaveCardBillingAddress(cardInfo);
                            message = cardInfo.MerchantReferenceCode;
                            // send text message Future pay 
                            if (message.Length == 10)
                            {
                                textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, (Convert.ToDecimal(cardInfo.Amount) ).ToString(), cardInfo.AccountNumber, cardInfo.FuturePaymentDate, message);
                                var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                            }
                        }
                        else if (CardDetails != null)
                        {
                            cardInfo.TokenId = CardDetails.TokenId;
                            cardPayment.SaveCardInfo(cardInfo);
                            cardPayment.SaveCardBillingAddress(cardInfo);
                            message = cardInfo.MerchantReferenceCode;

                            // send text message Future pay 
                            if (message.Length == 10)
                            {
                                textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, (Convert.ToDecimal(cardInfo.Amount) ).ToString(), cardInfo.AccountNumber, cardInfo.FuturePaymentDate, message);
                                var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                            }
                        }
                        else
                        {                            
                            status = CyberInfo.TokenizationSubscription(cardInfo);
                            message = ReturnMessageCardPayment(status, cardInfo, MAF.BAL.ResourceFile.Common.CardSubscription);
                            cardInfo.TokenId = tokenId;
                            if (status.Substring(0, 3) == "100")
                            {
                                cardPayment.SaveCardInfo(cardInfo);
                                cardPayment.SaveCardBillingAddress(cardInfo);

                                // send text message Future pay 
                                textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, (Convert.ToDecimal(cardInfo.Amount)).ToString(), cardInfo.AccountNumber, cardInfo.FuturePaymentDate, message);
                                var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);

                            }
                        }
                        return Ok(message);

                    }
                    else return Ok("Your card has been blocked for this merchant for a day. Please try another card.");
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in future pay Controller in Card Payment Action Method", ex);
                return null;
            }
        }


        private string ReturnMessageCardPayment(string status, CardInfoEntity cardInfoEntity,string Description)
        {
            CardDetailsEntity cardInfo = new CardDetailsEntity();
            string textMessage = string.Empty;
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
                    cardPayment.SaveCardTransactionLog(cardInfo);
                    return cardInfoEntity.MerchantReferenceCode; ;
                }
                else
                {
                    cardInfo.ConfirmationId = string.Empty;
                    cardInfo.TokenId = string.Empty;
                    cardInfo.RequestId = status.Substring(4, 22);
                    cardInfo.Description = status.Remove(0, 27);
                    cardPayment.SaveCardTransactionLog(cardInfo);
                    return status.Remove(0, 27);

                }
            }
            return string.Empty;
        }

        #endregion

        #endregion

        #region Fee
        /// <summary>
        /// Method to get fee based on payment method
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFee/{paymentMethod:int}/{accountNumber:int}")]
        public HttpResponseMessage GetFee(int paymentMethod, int accountNumber)
        {
            try
            {
                string fee = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Account Number cannot be blank.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(paymentMethod)))
                    {
                        //Get Fee based on payment method and account number
                        fee = utility.GetFeeByPaymentMethodAndPaymentType(Convert.ToString(accountNumber), Convert.ToString(paymentMethod) == "1" ? "ACH" : "Card", "OneTime");//paymentMethod =1=ACH,2=Card

                        return Request.CreateResponse(HttpStatusCode.OK, new AutoPayAgreementEntity { Fee = fee });
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Invalid payment method.");
                    }
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Get Fee Method.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }
        #endregion

        private bool ValidateFuturePayDate(DateTime futurePayDate, DateTime dueDate, DateTime nextDueDate)
        {
            bool isValid = false;
            try
            {
                if (futurePayDate > dueDate && futurePayDate <= nextDueDate)
                {
                    isValid = true;
                }
                return isValid;
            }
            catch
            {
                throw;
            }
        }
    }
}