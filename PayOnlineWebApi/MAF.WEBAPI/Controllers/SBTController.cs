using MAF.BAL;
using MAF.ENTITY;
using MAF.ENTITY.SolutionByText;
using MAF.SolutionByText;
using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using CyberSource.CyberService;
using MAF.ENTITY.CyberSource;
using System.Net.Http;
using System.Net;
using System.Configuration;

namespace MAF.WEBAPI.Controllers
{
     [Authorize]
    [RoutePrefix("api/SolutionsByText")]
    public class SBTController : ApiController
    {
        //Intialize interface members
        private readonly IDataContext dataContext;
        private readonly IPayByText payByText;
        private readonly IUtility utility;
        private readonly IAchPayment achPayment;
        private readonly ICardPayment cardPayment;
        private readonly IAutoPay autoPay;

        public SBTController(IDataContext dataContext, IPayByText payByText, IUtility utility, IAchPayment achPayment, ICardPayment cardPayment, IAutoPay autoPay)
        {
            this.dataContext = dataContext;
            this.payByText = payByText;
            this.utility = utility;
            this.achPayment = achPayment;
            this.cardPayment = cardPayment;
            this.autoPay = autoPay;
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

        [HttpPost]
        [Route("StatusUrl")]
        public void StatusUrl()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine("Start Status url");

                StatusUrlInfoEntity statusUrlInfo = new StatusUrlInfoEntity
                {
                    Code = HttpContext.Current.Request["code"],
                    Message = HttpContext.Current.Request["message"],
                    SendTo = HttpContext.Current.Request["sendTo"],
                    OrgCode = HttpContext.Current.Request["orgcode"],
                    Status = HttpContext.Current.Request["status"],
                    //// unique id for the transaction
                    Ticket = HttpContext.Current.Request["ticket"],
                    Note = HttpContext.Current.Request["note"],
                    UniqueId = HttpContext.Current.Request["UniqueID"],
                    MessageCategory = HttpContext.Current.Request["MessageCategory"],
                    MessageSubCategory = HttpContext.Current.Request["MessageSubcategory"]
                };

                string result = dataContext.SaveStatusUrlInfo(statusUrlInfo);
                sb.AppendLine(result);
                sb.AppendLine("End Status url");

            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in parsing StatusUrl response", ex);
                //throw ex;
            }
            finally
            {
                MAF.BAL.Logger.WriteTraceLog(sb.ToString());
            }
        }

        [HttpPost]
        [Route("CallbackUrl")]
        public void Callback()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine("Start Callback url");

                CallbackUrlInfoEntity callbackUrlInfo = new CallbackUrlInfoEntity
                {
                    Code = HttpContext.Current.Request["code"],
                    Message = HttpContext.Current.Request["message"],
                    Title = HttpContext.Current.Request["title"],
                    ShortCode = HttpContext.Current.Request["shortCode"],
                    Phone = HttpContext.Current.Request["phone"],
                    Carrier = HttpContext.Current.Request["carrier"],
                    Note = HttpContext.Current.Request["note"],
                    UniqueId = HttpContext.Current.Request["UniqueID"],
                    Keyword = HttpContext.Current.Request["keyword"],
                    DefaultKeyword = HttpContext.Current.Request["default_keyword"],
                    Group = HttpContext.Current.Request["group"]
                };

                string result = dataContext.SaveCallbackUrlInfo(callbackUrlInfo);

                //Process Pay By Text based on the reply
                ProcessPayByText(callbackUrlInfo);

                //Opt Out from Pay By Text
                OptOutPayByText(callbackUrlInfo);

                //Opt Out from AutoPay
                OptOutAutoPay(callbackUrlInfo);

                sb.AppendLine(result);
                sb.AppendLine("End Callback url");
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in parsing CallBackUrl response", ex);
                //throw ex;
            }
            finally
            {
                MAF.BAL.Logger.WriteTraceLog(sb.ToString());
            }
        }

        private void ProcessPayByText(CallbackUrlInfoEntity callBackUrlInfo)
        {
            try
            {
                PayByTextEntity payByTextEntity = new PayByTextEntity();
                List<PayByTextMessageLogEntity> listPayByTextMessageLog = new List<PayByTextMessageLogEntity>();

                payByTextEntity = payByText.GetActivePayByTextSetup().Where(l => l.AcctNo == callBackUrlInfo.UniqueId).FirstOrDefault();

                if (payByTextEntity != null)
                {
                    if (callBackUrlInfo.Phone.Contains(payByTextEntity.TextNumber))
                    {
                        //Check if the reply keyword is 'PAY' for Pay By Text then Send Approval Message
                        if (callBackUrlInfo.Keyword.Equals("PAY", StringComparison.OrdinalIgnoreCase))
                        {

                            listPayByTextMessageLog = payByText.GetPayByTextMessageLog(callBackUrlInfo.UniqueId, payByTextEntity.TextNumber)
                                .Where(l => l.Keyword.Equals("PAY", StringComparison.OrdinalIgnoreCase) && l.Type.Equals("SEND", StringComparison.OrdinalIgnoreCase)).ToList();

                            if (listPayByTextMessageLog.Count > 0)
                            {
                                //Save log
                                PayByTextMessageLogEntity log = new PayByTextMessageLogEntity()
                                {
                                    AcctNo = payByTextEntity.AcctNo,
                                    TextNumber = payByTextEntity.TextNumber,
                                    Message = callBackUrlInfo.Message,
                                    Type = "REPLY",
                                    Keyword = "PAY"
                                };

                                //save pay by text message log
                                payByText.SavePayByTextMessageLog(log);

                                //Send confirm message
                                SendPayByTextMessage(payByTextEntity, "CONFIRM");
                            }
                            else
                            {
                                SendPayByTextMessage(payByTextEntity, "PAY");
                            }
                        }

                        //Check if the reply keyword is 'CONFIRM' for Pay By Text then Process the payment
                        if (callBackUrlInfo.Keyword.Equals("CONFIRM", StringComparison.OrdinalIgnoreCase))
                        {
                            listPayByTextMessageLog = payByText.GetPayByTextMessageLog(callBackUrlInfo.UniqueId, payByTextEntity.TextNumber)
                                .Where(l => l.Keyword.Equals("CONFIRM", StringComparison.OrdinalIgnoreCase) && l.Type.Equals("SEND", StringComparison.OrdinalIgnoreCase)).ToList();

                            if (listPayByTextMessageLog.Count > 0)
                            {
                                //Save log
                                PayByTextMessageLogEntity log = new PayByTextMessageLogEntity()
                                {
                                    AcctNo = payByTextEntity.AcctNo,
                                    TextNumber = payByTextEntity.TextNumber,
                                    Message = callBackUrlInfo.Message,
                                    Type = "REPLY",
                                    Keyword = "CONFIRM"
                                };

                                //save pay by text message log
                                payByText.SavePayByTextMessageLog(log);

                                //Process Card Payments
                                if (string.IsNullOrWhiteSpace(payByTextEntity.BankABA))
                                {
                                    ProcessCardPayment(payByTextEntity);
                                }
                                //Process ACH Payments
                                else
                                {
                                    ProcessACHPayment(payByTextEntity);
                                }

                                SendPayByTextMessage(payByTextEntity, "CONFIRM");
                            }
                            else
                            {
                                SendPayByTextMessage(payByTextEntity, "CONFIRM");
                            }
                        }
                    }
                }

                
            }
            catch
            {
                throw;
            }
        }

        private void SendPayByTextMessage(PayByTextEntity payByTextEntity, string keyword)
        {
            try
            {
                string textMessage = string.Empty, type = string.Empty;

                var paymentMethod = string.IsNullOrWhiteSpace(payByTextEntity.BankABA) ? "Card" : "ACH";

                //Get fee
                string fee = utility.GetFeeByPaymentMethodAndPaymentType(payByTextEntity.AcctNo, paymentMethod, "OneTime");

                decimal totalAmountDue = payByTextEntity.DueAmount + Convert.ToDecimal(fee);

                //get text message
                if (keyword.Equals("PAY", StringComparison.OrdinalIgnoreCase))
                {
                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextPay, payByTextEntity.AcctNo, totalAmountDue.ToString(), payByTextEntity.DueDate.ToString("{0:MM/dd/yyyy}"));
                }
                else if (keyword.Equals("CONFIRM", StringComparison.OrdinalIgnoreCase))
                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextConfirm, totalAmountDue.ToString(), payByTextEntity.BankAcctNo);

                ResponseMessage response = Helper.Instance.SendTextMessageToTextNumber(payByTextEntity.AcctNo, payByTextEntity.TextNumber, MAF.BAL.ResourceFile.TextMessages.PayByTextPay);

                if (response.IsSuccess)
                {
                    PayByTextMessageLogEntity log = new PayByTextMessageLogEntity()
                    {
                        AcctNo = payByTextEntity.AcctNo,
                        TextNumber = payByTextEntity.TextNumber,
                        Message = textMessage,
                        Type = "SEND",
                        Keyword = keyword
                    };

                    //save pay by text message log
                    payByText.SavePayByTextMessageLog(log);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProcessACHPayment(PayByTextEntity payByTextEntity)
        {
            try
            {
                string amount = string.Empty, textMessage = string.Empty, confirmationNumber = string.Empty;
                var message = string.Empty;

                PaymentConfirmationEntity achPaymentInfo = new PaymentConfirmationEntity();

                confirmationNumber = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));


                //Get fee
                string fee = utility.GetFeeByPaymentMethodAndPaymentType(payByTextEntity.AcctNo, "ACH", "OneTime");

                achPaymentInfo.AccountNumber = payByTextEntity.AcctNo;
                achPaymentInfo.PaymentAmount = payByTextEntity.DueAmount.ToString();
                achPaymentInfo.FeeAmoun = fee;
                achPaymentInfo.RountingNumber = payByTextEntity.BankABA;
                achPaymentInfo.BankAccountNumber = payByTextEntity.BankAcctNo;
                achPaymentInfo.BankName = payByTextEntity.BankName;
                achPaymentInfo.BankAccountName = payByTextEntity.BankHolder;
                achPaymentInfo.AccountType = payByTextEntity.BankAcctType;
                achPaymentInfo.ConfirmationId = confirmationNumber;
                message = achPayment.SavePayment(achPaymentInfo);
                amount = (achPaymentInfo.PaymentAmount + achPaymentInfo.FeeAmoun).ToString();
                if (message.Length == 10)
                {
                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulScheduledPayment, amount, achPaymentInfo.AccountNumber, confirmationNumber);
                    Helper.Instance.SendPaymentConfirmationTextMessage(achPaymentInfo.AccountNumber, textMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProcessCardPayment(PayByTextEntity payByTextEntity)
        {
            try
            {
                string amount = string.Empty, textMessage = string.Empty, confirmationNumber = string.Empty;
                var message = string.Empty;
                CardDetailsEntity cardDetailsEntity = new CardDetailsEntity();

                CardInfoEntity cardInfo = new CardInfoEntity();

                //Get fee
                string fee = utility.GetFeeByPaymentMethodAndPaymentType(payByTextEntity.AcctNo, "Card", "OneTime");

                cardDetailsEntity = cardPayment.CardDetails(payByTextEntity.AcctNo).Where(s => s.TokenId == payByTextEntity.SubscriptionId).FirstOrDefault();

                confirmationNumber = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));

                cardDetailsEntity.ConfirmationId = confirmationNumber;

                cardInfo.AccountNumber = cardDetailsEntity.AcctNo;
                cardInfo.CardName = cardDetailsEntity.CardName;
                cardInfo.CardNumber = cardDetailsEntity.CardNumber;
                cardInfo.CardType = cardDetailsEntity.CardType;
                cardInfo.CardExpiry = cardDetailsEntity.CardExpiry;

                cardInfo.CardType = cardInfo.CardType == "V" ? "001" : "002";
                cardInfo.MerchantReferenceCode = confirmationNumber;
                amount = (Convert.ToDecimal(payByTextEntity.DueAmount) + Convert.ToDecimal(fee)).ToString();
                cardInfo.Amount = amount;
                message = CyberInfo.RePayment(cardInfo);
                cardInfo.TokenId = string.Empty;
                cardDetailsEntity.ReasonCode = message.Substring(0, 3);
                if (message.Substring(0, 3) == "100")
                {
                    cardInfo.Status = "Completed";
                    cardDetailsEntity.Description = MAF.BAL.ResourceFile.Common.CardTransactionSuccessfull;
                    cardDetailsEntity.RequestId = message.Substring(4, 22);

                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulPayment, amount, cardDetailsEntity.AcctNo, confirmationNumber);
                }
                else
                {
                    cardInfo.Status = "Decline";
                    cardDetailsEntity.ConfirmationId = string.Empty;
                    cardDetailsEntity.RequestId = message.Substring(4, 22);
                    cardDetailsEntity.Description = message.Remove(0, 27);

                    textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.UnsuccessfulPayment, amount, cardDetailsEntity.AcctNo, "Due to " + cardDetailsEntity.Description + ". ");
                }

                cardPayment.SaveCardInfo(cardInfo);
                cardPayment.SaveCardTransactionLog(cardDetailsEntity);
                Helper.Instance.SendPaymentConfirmationTextMessage(cardDetailsEntity.AcctNo, textMessage);

            }
            catch
            {
                throw;
            }
        }

        private void OptOutPayByText(CallbackUrlInfoEntity callBackUrlInfo)
        {
            try
            {
                PayByTextEntity payByTextEntity = new PayByTextEntity();

                payByTextEntity = payByText.GetPayByTextSetup(callBackUrlInfo.UniqueId);

                if (payByTextEntity != null)
                {
                    if (callBackUrlInfo.Phone.Contains(payByTextEntity.TextNumber))
                    {
                        string message = callBackUrlInfo.Message;
                        //Check if the reply message contains is 'PAYBYTEXT OFF' for Pay By Text then delete pay by text
                        if (message.ToUpper().Contains(ConfigurationManager.AppSettings["OptOutPayByTextKeyword"].ToUpper()))
                        {
                            var result = payByText.DeletePayByTextSetup(callBackUrlInfo.UniqueId);
                            
                            // send text message 
                            if (result == "Pay By Text Setup is deleted successfully.")
                            {
                                //Update subscriber
                                HttpResponseMessage responseMessage = UpdateSubscriber(callBackUrlInfo.UniqueId);
                                string textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.DeletePayByText, callBackUrlInfo.UniqueId);

                                var response = Helper.Instance.SendTextMessageToTextNumber(callBackUrlInfo.UniqueId, payByTextEntity.TextNumber, textMessage);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void OptOutAutoPay(CallbackUrlInfoEntity callBackUrlInfo)
        {
            try
            {
                var autoPayDetail = autoPay.GetAutoPayDetail(callBackUrlInfo.UniqueId);

                if (autoPayDetail != null)
                {
                    string message = callBackUrlInfo.Message;
                    //Check if the reply message contains is 'AUTOPAY OFF' for Auto Pay then delete auto pay
                    if (message.ToUpper().Contains(ConfigurationManager.AppSettings["OptOutAutoPayKeyword"].ToUpper()))
                    {
                        // Delete Auto Pay Recurring
                        message = autoPay.DeleteAutoPay(callBackUrlInfo.UniqueId);

                        // send text message auto pay 
                        string textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.DeleteAutoPay, callBackUrlInfo.UniqueId);
                        var response = Helper.Instance.SendTextMessage(callBackUrlInfo.UniqueId, textMessage);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update subscriber additional notifications
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        private HttpResponseMessage UpdateSubscriber(string accountNumber)
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
                            //Save subscriber detail in db
                            string result = dataContext.UpdateAdditionalNotifications(subscriber.SubscriberID, subscriber.IsPaymentReminderNotification, subscriber.IsPaymentConfirmationNotification, isPayByTextNotifications, subscriber.IsCommunicationByTextNotification);
                            dataContext.SaveTextLog(accountNumber, subscriber.TextNumber, "AdditionalNotifications", "Updated additional notifications.", "Customer Via SMS");
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
