using CyberSource.CyberService;
using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using MAF.SolutionByText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
     [Authorize]
     [RoutePrefix("api/PaymentConfirmation")]
    public class PaymentConfirmationController : ApiController
    {
        // Get interface Payment Confirmation
         private readonly IPaymentConfirmation IPaymentConfirmation;
         private readonly IUtility utility;
        private readonly IAchPayment achPayment;
        private readonly ICardPayment cardPayment;

        public PaymentConfirmationController(IPaymentConfirmation iPaymentConfirmation, IUtility utility, IAchPayment achPayment, ICardPayment cardPayment)
         {
             // Create object Payment Confirmation 
            IPaymentConfirmation = iPaymentConfirmation;
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

         // Get one time ach fee
         [HttpGet]
         [Route("GET/{AccountNumber:int}")]
         public IHttpActionResult GET(int AccountNumber)
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
                     ////Pass the parameter account number into the Dealer Fee get
                     //message = IPaymentConfirmation.AchFee(Convert.ToString(AccountNumber));
                     //Get Fee based on payment method and account number
                     message = utility.GetFeeByPaymentMethodAndPaymentType(Convert.ToString(AccountNumber), "ACH", "OneTime");

                     //send web api Dealer Fee
                     return Ok(message);
                 }
             }
             catch (Exception ex)
             {
             MAF.BAL.Logger.WriteErrorLog("Error in PaymentConfirmation Controller Get one time ach fee Details Action Method", ex);
                 return null;
             }
         }

         // Get one time card fee
         [HttpGet]
         [Route("GETCardFee/{AccountNumber:int}")]
         public IHttpActionResult GetCardFee(int AccountNumber)
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
                     ////Pass the parameter account number into the get card  Fee
                     //message = IPaymentConfirmation.cardFee(Convert.ToString(AccountNumber));

                     //Get Fee based on payment method and account number
                     message = utility.GetFeeByPaymentMethodAndPaymentType(Convert.ToString(AccountNumber), "Card", "OneTime");
                     //send web api  Fee
                     return Ok(message);
                 }
             }
             catch (Exception ex)
             {
                MAF.BAL.Logger.WriteErrorLog("Error in PaymentConfirmation Controller Get one time card fee Details Action Method", ex);
                 return null;
             }
         }


         // This Post return the Payment Confirmation view page.
         [HttpPost]
         [Route("Post")]
         public IHttpActionResult Post(PaymentConfirmationEntity paymentConfirmation)
         {
            
             try
             {
                 string textMessage = string.Empty;
                 string message = string.Empty;
                 if (paymentConfirmation == null)
                 {
                     message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return BadRequest(message);

                 }

                 if (string.IsNullOrWhiteSpace(paymentConfirmation.AccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.PaymentAmount) || string.IsNullOrWhiteSpace(paymentConfirmation.FeeAmoun) || string.IsNullOrWhiteSpace(paymentConfirmation.RountingNumber) ||
                     string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountNumber) || string.IsNullOrWhiteSpace(paymentConfirmation.BankName) || string.IsNullOrWhiteSpace(paymentConfirmation.BankHolder) || string.IsNullOrWhiteSpace(paymentConfirmation.AccountType) ||
                     string.IsNullOrWhiteSpace(paymentConfirmation.UpdatedPhone) || string.IsNullOrWhiteSpace(paymentConfirmation.UpdatedPhoneDate) || string.IsNullOrWhiteSpace(paymentConfirmation.Email) || string.IsNullOrWhiteSpace(paymentConfirmation.SaveAccountFuture) || string.IsNullOrWhiteSpace(paymentConfirmation.BankAccountName))
                 {
                     message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                     // Error message send
                     return BadRequest(message);
                 }
                 else
                 {
                     if (paymentConfirmation.FuturePaymentDate != "")
                     {
                         paymentConfirmation.SaveAccountFuture = "1";
                     }
                     //Pass the parameter paymentConfirmation models into the Confirmation
                      message = achPayment.SavePayment(paymentConfirmation);
                   // send text message one time payment
                      if (message.Length == 10 && paymentConfirmation.FuturePaymentDate == "")
                      {
                          textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, (Convert.ToDecimal(paymentConfirmation.PaymentAmount) + Convert.ToDecimal(paymentConfirmation.FeeAmoun)).ToString(), paymentConfirmation.AccountNumber, message);
                         var response = Helper.Instance.SendPaymentConfirmationTextMessage(paymentConfirmation.AccountNumber, textMessage);
                        
                      }
                      // send text message one time Future payment
                      if (message.Length == 10 && paymentConfirmation.FuturePaymentDate != "")
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
                 MAF.BAL.Logger.WriteErrorLog("Error in PaymentConfirmation Controller in Post Details Action Method", ex);
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
             string textMessage = string.Empty;
             try
             {
                 var cardDetailsList = new List<CardDetailsEntity>();

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
                         cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); //HttpContext.Current.Request.UserHostAddress;
                         cardInfo.CardType = cardInfo.CardType == "Visa" ? "001" : "002";
                         if (!string.IsNullOrWhiteSpace(cardInfo.FuturePaymentDate))
                         {
                             var checkCardType = string.Empty;                             
                             cardDetailsList = cardPayment.CardDetails(cardInfo.AccountNumber);
                             var CardDetails = (from p in cardDetailsList where (p.CardNumber == cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4) && p.CardType == checkCardType && p.CardExpiry == cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear) select p).FirstOrDefault();
                             if (CardDetails == null)
                             {

                                 status = CyberInfo.TokenizationSubscription(cardInfo);
                                 message = ReturnMessageCardPayment(status, "Subscription", cardInfo, MAF.BAL.ResourceFile.Common.CardSubscription);
                                 cardInfo.TokenId = tokenId;
                                 if (status.Substring(0, 3) == "100")
                                 {
                                     cardPayment.SaveCardInfo(cardInfo);
                                     cardPayment.SaveCardBillingAddress(cardInfo);                                  
                                 }
                             }
                             else
                             {
                                 cardInfo.TokenId = CardDetails.TokenId;
                                 cardPayment.SaveCardInfo(cardInfo);
                                 cardPayment.SaveCardBillingAddress(cardInfo);
                                 message = cardInfo.MerchantReferenceCode;
                             }

                             // send text message one time Future payment
                             textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.FuturePaymentDate, cardInfo.MerchantReferenceCode);
                             var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                             return Ok(message);
                         }
                         else
                         {
                             if (cardInfo.SaveFuture == "True")
                             {
                                 var checkCardType = string.Empty;                                 
                                 cardDetailsList = cardPayment.CardDetails(cardInfo.AccountNumber);
                                 var CardDetails = (from p in cardDetailsList where (p.CardNumber == cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4) && p.CardType == checkCardType && p.CardExpiry == cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear) select p).FirstOrDefault();

                                 if (CardDetails != null)
                                 {

                                     status = CyberInfo.TokenizationPayments(cardInfo);
                                     message = ReturnMessageCardPayment(status, null, cardInfo, MAF.BAL.ResourceFile.Common.CardTransactionSuccessfull);
                                     cardInfo.TokenId = string.Empty;
                                     if (status.Substring(0, 3) == "100")
                                     {
                                         cardInfo.Status = "Completed";
                                         cardPayment.SaveCardInfo(cardInfo);
                                         cardPayment.SaveCardBillingAddress(cardInfo);
                                         SendMail.Instance.PaymentConfirmation(cardInfo.Amount, cardInfo.MerchantReferenceCode, cardInfo.Email, cardInfo.FirstName+" "+cardInfo.LastName);
                                         // send text message one time payment
                                         textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.MerchantReferenceCode);
                                         var response = Helper.Instance.SendPaymentConfirmationTextMessage(cardInfo.AccountNumber, textMessage);
                                     }
                                     else
                                     {
                                         cardInfo.Status = "Decline";
                                         cardPayment.SaveCardInfo(cardInfo);
                                     }
                                 }
                                 else
                                 {
                                     status = CyberInfo.TokenizationPaymentsSubscription(cardInfo);
                                     message = ReturnMessageCardPayment(status, "Subscription", cardInfo, MAF.BAL.ResourceFile.Common.CardTransactionSuccessfull);

                                     if (status.Substring(0, 3) == "100")
                                     {
                                         cardInfo.TokenId = tokenId;
                                         cardInfo.Status = "Completed";
                                         cardPayment.SaveCardInfo(cardInfo);
                                         cardPayment.SaveCardBillingAddress(cardInfo);
                                         SendMail.Instance.PaymentConfirmation(cardInfo.Amount, cardInfo.MerchantReferenceCode, cardInfo.Email, cardInfo.FirstName + " " + cardInfo.LastName);
                                         // send text message one time payment
                                         textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.MerchantReferenceCode);
                                         var response = Helper.Instance.SendPaymentConfirmationTextMessage(cardInfo.AccountNumber, textMessage);

                                     }
                                     else
                                     {
                                         cardInfo.TokenId = string.Empty;
                                         cardInfo.Status = "Decline";
                                         cardPayment.SaveCardInfo(cardInfo);
                                     }
                                 }
                                 return Ok(message);
                             }
                             else
                             {

                                 status = CyberInfo.TokenizationPayments(cardInfo);
                                 message = ReturnMessageCardPayment(status, null, cardInfo, MAF.BAL.ResourceFile.Common.CardTransactionSuccessfull);
                                 cardInfo.TokenId = string.Empty;
                                 if (status.Substring(0, 3) == "100")
                                 {
                                     cardInfo.Status = "Completed";
                                     cardPayment.SaveCardInfo(cardInfo);
                                     cardPayment.SaveCardBillingAddress(cardInfo);
                                     SendMail.Instance.PaymentConfirmation(cardInfo.Amount, cardInfo.MerchantReferenceCode, cardInfo.Email, cardInfo.FirstName + " " + cardInfo.LastName);
                                     textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.MerchantReferenceCode);
                                     Helper.Instance.SendPaymentConfirmationTextMessage(cardInfo.AccountNumber, textMessage);
                                 }
                                 else
                                 {
                                     cardInfo.Status = "Decline";
                                     cardPayment.SaveCardInfo(cardInfo);
                                 }
                                 return Ok(message);
                             }

                         }
                     }
                     else return Ok("Your card has been blocked for this merchant for a day. Please try another card.");
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Card Payment Action Method", ex);
                 return null;
             }
         }

         //SavedCard Payment  function 
         [HttpPost]
         [Route("SavedCardPayment")]
         public IHttpActionResult SavedCardPayment(CardInfoEntity cardInfo)
         {
              string textMessage = string.Empty;
             try
             {
                 if (string.IsNullOrWhiteSpace(cardInfo.AccountNumber) || string.IsNullOrWhiteSpace(cardInfo.FirstName) || string.IsNullOrWhiteSpace(cardInfo.LastName) || string.IsNullOrWhiteSpace(cardInfo.Street) || string.IsNullOrWhiteSpace(cardInfo.City)
                     || string.IsNullOrWhiteSpace(cardInfo.State) || string.IsNullOrWhiteSpace(cardInfo.PostelCode) || string.IsNullOrWhiteSpace(cardInfo.Email) ||
                     string.IsNullOrWhiteSpace(cardInfo.CardNumber) || string.IsNullOrWhiteSpace(cardInfo.ExpirationMonth) || string.IsNullOrWhiteSpace(cardInfo.ExpirationYear) ||
                     string.IsNullOrWhiteSpace(cardInfo.CardType) || string.IsNullOrWhiteSpace(cardInfo.Amount) || string.IsNullOrWhiteSpace(cardInfo.CvNumber) || string.IsNullOrWhiteSpace(cardInfo.CardName))
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
                         cardInfo.IpAddress = Helper.Instance.GetClientIp(Request); //HttpContext.Current.Request.UserHostAddress;
                         cardInfo.CardType = cardInfo.CardType == "Visa" ? "001" : "002";
                         if (!string.IsNullOrWhiteSpace(cardInfo.FuturePaymentDate))
                         {
                             cardPayment.SaveCardBillingAddress(cardInfo);
                             cardPayment.SaveCardInfo(cardInfo);
                             // send text message one time Future payment
                             textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.FuturePay, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.FuturePaymentDate, cardInfo.MerchantReferenceCode);
                             var response = Helper.Instance.SendTextMessage(cardInfo.AccountNumber, textMessage);
                             return Ok(cardInfo.MerchantReferenceCode);
                         }
                         else
                         {
                             status = CyberInfo.RePayment(cardInfo);
                             if (status.Substring(0, 3) == "100")
                             {
                                 cardInfo.Status = "Completed";
                                 cardPayment.SaveCardInfo(cardInfo);
                                 cardPayment.SaveCardBillingAddress(cardInfo);
                                 SendMail.Instance.PaymentConfirmation(cardInfo.Amount, cardInfo.MerchantReferenceCode, cardInfo.Email, cardInfo.FirstName + " " + cardInfo.LastName);
                                 // send text message one time  payment
                                 textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, cardInfo.Amount, cardInfo.AccountNumber, cardInfo.MerchantReferenceCode);
                                 var response = Helper.Instance.SendPaymentConfirmationTextMessage(cardInfo.AccountNumber, textMessage);
                             }
                             else
                             {
                                 cardInfo.Status = "Decline";
                                 cardPayment.SaveCardInfo(cardInfo);
                             }
                             message = ReturnMessageSavedCardPayment(status, cardInfo, MAF.BAL.ResourceFile.Common.CardSubscription);
                             return Ok(message);
                         }
                     }
                     else return Ok("Your card has been blocked for this merchant for a day. Please try another card.");
                 }
             }
             catch (Exception ex)
             {
                 MAF.BAL.Logger.WriteErrorLog("Error in PayOnline Controller in Saved Card Payment Action Method", ex);
                 return null;
             }
         }
         
         private string ReturnMessageCardPayment(string status , string TokenValue,CardInfoEntity cardInfoEntity,string Description)
         {
             CardDetailsEntity cardInfo= new CardDetailsEntity();
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
                     if (TokenValue == "Subscription") 
                     {
                         tokenId = status.Substring(status.Length - 16);
                     }
                     cardInfo.TokenId = tokenId;
                     cardInfo.ConfirmationId = cardInfoEntity.MerchantReferenceCode;
                     cardInfo.Description = Description;
                     cardInfo.RequestId = status.Substring(4, 22);
                     cardPayment.SaveCardTransactionLog(cardInfo);
                     return cardInfoEntity.MerchantReferenceCode; 
                 }
                 else
                 {
                     cardInfo.ConfirmationId = string.Empty;
                     cardInfo.TokenId = string.Empty;
                     cardInfo.RequestId = status.Substring(4, 22);
                     cardInfo.Description = status.Remove(0, 27);
                     cardPayment.SaveCardTransactionLog(cardInfo);
                     textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.UnsuccessfulPayment, cardInfoEntity.Amount, cardInfo.AcctNo, "Due to " + cardInfo.Description + ". ");
                     Helper.Instance.SendPaymentConfirmationTextMessage(cardInfo.AcctNo, textMessage);
              
                     return status.Remove(0, 27);
                     
                 }                 
             }
             return string.Empty;
         }

         private string ReturnMessageSavedCardPayment(string status, CardInfoEntity cardInfoEntity,string Description)
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
                 cardInfo.CardExpiry = cardInfoEntity.ExpirationMonth+"/"+cardInfoEntity.ExpirationYear;
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
