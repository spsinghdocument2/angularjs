using CyberSource.CyberService;
using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using MAF.ENTITY.SolutionByText;
using MAF.SolutionByText;
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
    [RoutePrefix("api/PayByText")]
    public class PayByTextController : ApiController
    {
        private readonly IFuturePay IFuturePay;
        private readonly IAchPayment achPayment;
        private readonly ICardPayment cardPayment;
        private readonly IUtility utility;
        private readonly IDataContext dataContext;
        private readonly IPayByText payByText;
        private readonly IPayOnline payOnline;

        FuturePayEntity futurePayEntity = null;
        public PayByTextController(IFuturePay iFuturePay, IUtility utility, IPayByText payByText, IDataContext dataContext, IAchPayment achPayment, ICardPayment cardPayment, IPayOnline payOnline)
        {
            IFuturePay = iFuturePay;
            this.utility = utility;
            this.payByText = payByText;
            this.dataContext = dataContext;
            this.achPayment = achPayment;
            this.cardPayment = cardPayment;
            this.payOnline= payOnline;
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

        //Get the Account Information  function
        [HttpGet]
        [Route("GetAccountInfo/{AccountNumber:int}")]
        public IHttpActionResult GetAccountInfo(int AccountNumber)
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
                    futurePayEntity = IFuturePay.GetAccountInfo(AccountNumber);
                    return Ok(futurePayEntity);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in future Pay Controller Get account Details Action Method", ex);
                return null;
            }
        }

        /// <summary>
        /// Method to get pay by text setup
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSetup/{accountNumber}")]
        public HttpResponseMessage GetPayByTextSetup(string accountNumber)
        {
            try
            {
                string fee = string.Empty;
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Account Number cannot be blank.");
                }
                else
                {
                    PayByTextEntity payByTextEntity = new PayByTextEntity();

                    payByTextEntity = payByText.GetPayByTextSetup(accountNumber);
                    return Request.CreateResponse(HttpStatusCode.OK, payByTextEntity);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Get Pay By Text Setup.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Method to delete pay by text setup
        /// </summary>
        /// <param name="accountNumber">account number</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeletePayByText/{accountNumber}")]
        public HttpResponseMessage DeletePayByTextSetup(string accountNumber)
        {
            try
            {
                string message = string.Empty;
                string textMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Account Number cannot be blank.");
                }
                else
                {
                    PayByTextEntity payByTextEntity = new PayByTextEntity();

                    payByTextEntity = payByText.GetPayByTextSetup(accountNumber);

                    if (payByTextEntity != null)
                    {
                        message = payByText.DeletePayByTextSetup(accountNumber);
                        // send text message 
                        if (message == "Pay By Text Setup is deleted successfully.")
                        {
                            ////Update subscriber
                            HttpResponseMessage responseMessage = UpdateSubscriber(accountNumber, string.Empty, false);
                            textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.DeletePayByText, accountNumber);

                            var response = Helper.Instance.SendTextMessageToTextNumber(accountNumber, payByTextEntity.TextNumber, textMessage);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Get Fee Method.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

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
        /// <param name="accountNumber">Account Number</param>
        /// /// <param name="accountNumber">Id</param>
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
                RoutingNumberEntity routingNumberEntity = new RoutingNumberEntity();
                if (accountInformation == null)
                {
                    string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);
                }

                if (string.IsNullOrWhiteSpace(accountInformation.AccountNumber))
                {
                    string message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);
                }
                else
                {
                    var NSFStatus = payOnline.CheckNSFStatus(accountInformation.AccountNumber);
                    if (!string.IsNullOrWhiteSpace(accountInformation.Routing) && accountInformation.PaymentMethod == "ACH")
                    {
                        if (NSFStatus == "ABlocked" || NSFStatus == "Blocked")
                        {
                            routingNumberEntity.Error = MAF.BAL.ResourceFile.Common.LoginAcceptedPaymentsFaild;
                        }
                        else
                        {
                            //Pass the parameter value into the routing number verify
                            routingNumberEntity = achPayment.ValidateRoutingNumber(accountInformation.Routing);
                        }
                    }
                    if (accountInformation.PaymentMethod == "Card")
                    {
                        if (NSFStatus == "ABlocked"){routingNumberEntity.Error = MAF.BAL.ResourceFile.Common.LoginAcceptedPaymentsFaild;}                       
                    }
                    return Ok(routingNumberEntity);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Get Routing Number Action Method", ex);
                return null;
            }
        }

        [HttpPost]
        [Route("PayByTextACHSetup")]
        public HttpResponseMessage PayByTextSetupWithACH(PaymentConfirmationEntity paymentConfirmation)
        {
            try
            {
                string message = string.Empty;
                string textMessage = string.Empty;
                if (paymentConfirmation == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }

                if (string.IsNullOrWhiteSpace(paymentConfirmation.AccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.RountingNumber) ||
                    string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.BankName) || string.IsNullOrWhiteSpace(paymentConfirmation.BankHolder) || string.IsNullOrWhiteSpace(paymentConfirmation.AccountType) ||
                    string.IsNullOrWhiteSpace(paymentConfirmation.UpdatedPhone) || string.IsNullOrWhiteSpace(paymentConfirmation.Email) || string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountName))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    //return BadRequest(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
                else
                {
                    
                    //get already existing pay by text setup
                    var payByTextSetup = payByText.GetPayByTextSetup(paymentConfirmation.AccountNumber);

                    if (payByTextSetup != null && !paymentConfirmation.EditStatus.Equals("True"))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You can not have multiple setup for Pay By Text. Please refresh the page.");
                    }

                    //Pass the parameter paymentConfirmation models into the Confirmation
                    message = payByText.SavePayByTextWithACH(paymentConfirmation);

                    // send text message 
                    if (message.Length == 10)
                    {
                        string bankNumber = paymentConfirmation.BankAccountNumber;
                        bankNumber = bankNumber.Substring(bankNumber.Length - 4);
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextEnrollment, paymentConfirmation.AccountNumber, bankNumber, message);
                        var textMessageResponse = Helper.Instance.SendTextMessage(paymentConfirmation.AccountNumber, textMessage);

                    }
                    //Update subscriber
                    HttpResponseMessage response  = UpdateSubscriber(paymentConfirmation.AccountNumber, paymentConfirmation.TextNumber, true);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, message); ;
                    }
                    //send web api Loan Payment
                    else
                    {
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Pay By Text Controller in Ach Setup Action Method", ex);
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
                MAF.BAL.Logger.WriteErrorLog("Error in Pay By Text Controller in Get Card Details Action Method", ex);
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
                MAF.BAL.Logger.WriteErrorLog("Error in Pay By Text Controller in Delete Card Details Action Method", ex);
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

        string status = string.Empty;
        string statusCode = string.Empty;
        string tokenId = string.Empty;
        string message = string.Empty;
        //Card Payment  function 
        [HttpPost]
        [Route("PayByTextCardSetup")]
        public HttpResponseMessage PayByTextSetupWithCard(CardInfoEntity cardInfo)
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
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    var futurePayEntity = IFuturePay.GetAccountInfo(Convert.ToInt32(cardInfo.AccountNumber));

                    //get already existing pay by text setup
                    var payByTextSetup = payByText.GetPayByTextSetup(cardInfo.AccountNumber);

                    if (payByTextSetup != null && !cardInfo.EditStatus.Equals("True"))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You can not have multiple setup for Pay By Text. Please refresh the page.");
                    }

                     var cardLimit = cardPayment.CheckCardMaxLimit(cardInfo);
                     if (cardLimit != "Blocked")
                     {
                         cardInfo.MerchantReferenceCode = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                         cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); // HttpContext.Current.Request.UserHostAddress;
                         cardInfo.Amount = (Convert.ToDecimal(cardInfo.Amount) + Convert.ToDecimal(cardInfo.Fee)).ToString();
                         var checkCardType = string.Empty;
                         checkCardType = cardInfo.CardType;                         
                         cardDetailsList = cardPayment.CardDetails(cardInfo.AccountNumber);
                         var CardDetails = (from p in cardDetailsList where (p.CardNumber == cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4) && p.CardType == checkCardType && p.CardExpiry == cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear) select p).FirstOrDefault();
                         cardInfo.CardType = checkCardType == "V" ? "001" : "002";
                         if (!string.IsNullOrEmpty(cardInfo.TokenId))
                         {
                             payByText.SavePayByTextWithCard(cardInfo, futurePayEntity.Name);
                             cardPayment.SaveCardBillingAddress(cardInfo);
                             message = cardInfo.MerchantReferenceCode;
                             // send text message 
                             if (message.Length == 10)
                             {
                                 textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextEnrollment, cardInfo.AccountNumber, cardInfo.CardNumber, message);
                                 var textMessageResponse = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);

                             }
                         }
                         else if (CardDetails != null)
                         {
                             cardInfo.TokenId = CardDetails.TokenId;
                             payByText.SavePayByTextWithCard(cardInfo, futurePayEntity.Name);
                             cardPayment.SaveCardBillingAddress(cardInfo);
                             message = cardInfo.MerchantReferenceCode;
                             // send text message 
                             if (message.Length == 10)
                             {
                                 string cardNumber = cardInfo.CardNumber;
                                 cardNumber = cardNumber.Substring(cardNumber.Length - 4);
                                 textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextEnrollment, cardInfo.AccountNumber, cardNumber, message);
                                 var textMessageResponse = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                             }
                         }
                         else
                         {
                             cardInfo.CardType = checkCardType == "V" ? "001" : "002";
                             status = CyberInfo.TokenizationSubscription(cardInfo);
                             message = ReturnMessageCardPayment(status, cardInfo, MAF.BAL.ResourceFile.Common.CardSubscription);
                             cardInfo.TokenId = tokenId;
                             if (status.Substring(0, 3) == "100")
                             {
                                 payByText.SavePayByTextWithCard(cardInfo, futurePayEntity.Name);
                                 cardPayment.SaveCardBillingAddress(cardInfo);
                                 // send text message 
                                 if (message.Length == 10)
                                 {
                                     string cardNumber = cardInfo.CardNumber;
                                     cardNumber = cardNumber.Substring(cardNumber.Length - 4);
                                     textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextEnrollment, cardInfo.AccountNumber, cardNumber, message);
                                     var textMessageResponse = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);

                                 }
                             }
                         }
                         //Update subscriber
                         HttpResponseMessage response = UpdateSubscriber(cardInfo.AccountNumber, cardInfo.TextNumber, true);

                         if (response.StatusCode == HttpStatusCode.OK)
                         {
                             return Request.CreateResponse(HttpStatusCode.OK, message); ;
                         }
                         //send web api Loan Payment
                         else
                         {
                             return response;
                         }
                     }
                     else return Request.CreateResponse(HttpStatusCode.OK, "Your card has been blocked for this merchant for a day. Please try another card.");
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Pay By Text Controller in Card Setup Action Method", ex);
                return null;
            }
        }

        private string ReturnMessageCardPayment(string status, CardInfoEntity cardInfoEntity, string Description)
        {
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

        /// <summary>
        /// Method to update subscriber additional notifications
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="textNumber"></param>
        /// <param name="isPayByTextActive"></param>
        /// <returns></returns>
        private HttpResponseMessage UpdateSubscriber(string accountNumber, string textNumber, bool isPayByTextActive)
        {
            try
            {
                //Get subscriber based on account number
                List<Subscriber> subscribers = new List<Subscriber>();
                subscribers = dataContext.GetSubscriberByAccountNumber(accountNumber); //.Where(s => s.TextNumber == textNumber).FirstOrDefault();

                if (subscribers.Count > 0)
                {
                    foreach (var subscriber in subscribers)
                    {
                        bool isPayByTextNotifications = false;
                        if (subscriber.IsActive)
                        {
                            if (isPayByTextActive)
                            {
                                //if text number of subscriber matches with the text number selected then enable 
                                //pay by text notifications otherwise disable it.
                                if (textNumber.Equals(subscriber.TextNumber))
                                {
                                    isPayByTextNotifications = true;
                                }
                            }
                            else
                                isPayByTextNotifications = false;

                            //Save subscriber detail in db
                            string result = dataContext.UpdateAdditionalNotifications(subscriber.SubscriberID, subscriber.IsPaymentReminderNotification, subscriber.IsPaymentConfirmationNotification, isPayByTextNotifications, subscriber.IsCommunicationByTextNotification);
                            dataContext.SaveTextLog(accountNumber, subscriber.TextNumber, "AdditionalNotifications", "Updated additional notifications.", subscriber.CreatedBy);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {

                throw;
            }
        }

    }
}

