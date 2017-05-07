using CyberSource.CyberService;
using MAF.BAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using MAF.SolutionByText;

namespace MAF.AutoPaymentProcessor
{
    public partial class ScheduledPaymentProcessingService : ServiceBase
    {
        public ScheduledPaymentProcessingService()
        {
            InitializeComponent();
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

        ///<summary>
        /// This private datamember is used to create object of CardPayment
        /// </summary>
        private CardPayment cardInfo;

        /// <summary>
        /// This property is used to create object of CardPayment class and check the object is already created or not
        /// </summary>
        public CardPayment CardInfo
        {
            get
            {
                if (cardInfo == null)
                {
                    cardInfo = new CardPayment();
                }
                return cardInfo;
            }
        }

        ///<summary>
        /// This private datamember is used to create object of AchPayment
        /// </summary>
        private AchPayment achInfo;

        /// <summary>
        /// This property is used to create object of AchPayment class and check the object is already created or not
        /// </summary>
        public AchPayment AchInfo
        {
            get
            {
                if (achInfo == null)
                {
                    achInfo = new AchPayment();
                }
                return achInfo;
            }
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();

            MAF.BAL.Logger.WriteTraceLog("ScheduledPaymentProcessing Service started.");
            this.ScheduleService();
            this.PayByTextService();
        }

        protected override void OnStop()
        {
            MAF.BAL.Logger.WriteTraceLog("ScheduledPaymentProcessing Service stopped.");
            this.Schedular.Dispose();
        }

        private Timer Schedular;
        private Timer PayByTextTimer;

        /// <summary>
        /// Method contains logic to run service functionality for Scheduled Payments based on mode
        /// </summary>
        void ScheduleService()
        {
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;
                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.                       
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                                   
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in ScheduledPaymentProcessing Timer Start.", ex);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("ScheduledPaymentProcessingService"))
                {
                    serviceController.Stop();
                }
            }
        }

        /// <summary>
        /// Method contains logic to run service functionality for Pay By Text based on the time
        /// </summary>
        void PayByTextService()
        {
            try
            {
                PayByTextTimer = new Timer(new TimerCallback(PayByTextCallback));

                //Set the Default Time.
                DateTime payByTextTime = DateTime.MinValue;

                //Get the Scheduled Time from AppSettings.
                payByTextTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["PayByTextTime"]);
                if (DateTime.Now > payByTextTime)
                {
                    //If Scheduled Time is passed set Schedule for the next day.                       
                    payByTextTime = payByTextTime.AddDays(1);
                }

                TimeSpan timeSpan = payByTextTime.Subtract(DateTime.Now);

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                PayByTextTimer.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in ScheduledPaymentProcessing Pay By Text Timer Start.", ex);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("ScheduledPaymentProcessingService"))
                {
                    serviceController.Stop();
                }
            }
        }
        
        /// <summary>
        /// Timer callback method
        /// </summary>
        /// <param name="e"></param>
        private void SchedularCallback(object e)
        {
            //Check for NSF Status
            CheckNotSufficientFundStatus();

            // Process ACH Payment
            ACHPayment();

            // Process Card Payment
            CardPayment();
            this.ScheduleService();
        }

        private void PayByTextCallback(object e)
        {
            //Initiate Pay By Text
            ProcessPayByText();

            this.PayByTextService();
        }

        /// <summary>
        /// Method to process scheduled ach payments
        /// </summary>
        private void ACHPayment()
        {
            try
            {
                string amount = string.Empty, textMessage = string.Empty, confirmation = string.Empty;
                var message = string.Empty;
                List<AchDetailEntity> achDetailsList = new List<AchDetailEntity>();
                PaymentConfirmationEntity achPaymentInfo = new PaymentConfirmationEntity();
                achDetailsList = AchInfo.GetScheduledTransactionsForAchPayment();

                decimal minAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                decimal maxAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]);

                foreach (var item in achDetailsList)
                {
                    decimal transAmount = Convert.ToDecimal(item.TranPayment);
                    if (transAmount < minAmount || transAmount > maxAmount)
                    {
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.ScheduledPaymentNotProcess, transAmount);
                        SendPaymentConfirmationTextMessage(achPaymentInfo.AccountNumber, textMessage);
                        continue;
                    }
                    confirmation = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                    achPaymentInfo.AccountNumber = item.AcctNo;
                    achPaymentInfo.PaymentAmount = item.TranPayment.ToString();
                    achPaymentInfo.FeeAmoun = item.TranFee.ToString();
                    achPaymentInfo.RountingNumber = item.BankABA;
                    achPaymentInfo.BankAccountNumber = item.BankAcctNo;
                    achPaymentInfo.BankName = item.BankName;
                    achPaymentInfo.BankAccountName = item.BankHolder;
                    achPaymentInfo.AccountType = item.BankAcctType;
                    achPaymentInfo.ConfirmationId = confirmation;
                    message = AchInfo.SavePayment(achPaymentInfo);
                    amount = (achPaymentInfo.PaymentAmount + achPaymentInfo.FeeAmoun).ToString();
                    if (message.Length == 10)
                    {
                        CardInfo.UpdateScheduleTransactionStatus(Convert.ToString(item.ID), "Completed");
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulScheduledPayment, amount, achPaymentInfo.AccountNumber, confirmation);
                        SendPaymentConfirmationTextMessage(achPaymentInfo.AccountNumber, textMessage);
                    }
                    else
                    {
                        CardInfo.UpdateScheduleTransactionStatus(Convert.ToString(item.ID), "Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Ach Payment Processing.", ex);
            }
        }

        /// <summary>
        /// Method to process scheduled card payments
        /// </summary>
        private void CardPayment()
        {
            try
            {
                var message = string.Empty;
                string confirmationId = string.Empty, textMessage=string.Empty;
                string amount = string.Empty;

                decimal minAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                decimal maxAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]);

                List<CardInfoEntity> cardInfoList = new List<CardInfoEntity>();
                CardDetailsEntity cardDetailsEntity = new CardDetailsEntity();
                cardInfoList = CardInfo.GetScheduledTransactionsForCardPayment();

                foreach (var item in cardInfoList)
                {
                    decimal transAmount = Convert.ToDecimal(item.Amount);
                    if (transAmount < minAmount || transAmount > maxAmount)
                    {
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.ScheduledPaymentNotProcess, transAmount);
                        SendPaymentConfirmationTextMessage(cardDetailsEntity.AcctNo, textMessage);
                        continue;
                    }
                    confirmationId = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
                    cardDetailsEntity.ConfirmationId = confirmationId;
                    cardDetailsEntity.AcctNo = item.AccountNumber;
                    cardDetailsEntity.CardName = item.CardName;
                    cardDetailsEntity.CardNumber = item.CardNumber;
                    cardDetailsEntity.CardType = item.CardType;
                    cardDetailsEntity.CardExpiry = item.CardExpiry;
                    cardDetailsEntity.TokenId = item.TokenId;
                    item.CardType = item.CardType == "V" ? "001" : "002";
                    item.MerchantReferenceCode = confirmationId;
                    amount = (Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.Fee)).ToString();
                    item.Amount = amount;                    
                    message = CyberInfo.RePayment(item);
                    item.TokenId = string.Empty;
                    cardDetailsEntity.ReasonCode = message.Substring(0, 3);
                    if (message.Substring(0, 3) == "100")
                    {
                        item.Status = "Completed";
                        cardDetailsEntity.Description = MAF.BAL.ResourceFile.Common.CardTransactionSuccessfull;
                        cardDetailsEntity.RequestId = message.Substring(4, 22);                        
                        CardInfo.SaveCardInfo(item);                        
                        CardInfo.SaveCardTransactionLog(cardDetailsEntity);
                        CardInfo.UpdateScheduleTransactionStatus(Convert.ToString(item.ScheduleID), "Completed");
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.SuccessfulScheduledPayment, amount, cardDetailsEntity.AcctNo, confirmationId);
                        SendPaymentConfirmationTextMessage(cardDetailsEntity.AcctNo, textMessage);
                    }
                    else
                    {
                        item.Status = "Decline";
                        cardDetailsEntity.ConfirmationId = string.Empty;
                        cardDetailsEntity.RequestId = message.Substring(4, 22);
                        cardDetailsEntity.Description = message.Remove(0, 27);
                        CardInfo.SaveCardInfo(item);
                        CardInfo.SaveCardTransactionLog(cardDetailsEntity);
                        CardInfo.UpdateScheduleTransactionStatus(Convert.ToString(item.ScheduleID), "Failed");
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.UnsuccessfulScheduledPayment, amount, cardDetailsEntity.AcctNo, "Due to " + cardDetailsEntity.Description+". ");
                        SendPaymentConfirmationTextMessage(cardDetailsEntity.AcctNo, textMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Card Payment Processing.", ex);
            }
        }

        /// <summary>
        /// Method to process NSF Status and STOP autopay or scheduled pay
        /// </summary>
        private void CheckNotSufficientFundStatus()
        { 
            try
            {
                CardInfo.UpdateMAFAccountConsecutiveNSFStatus();
            }
            catch (Exception ex)
            {
               MAF.BAL.Logger.WriteErrorLog("Error in Check Not Sufficient Fund Status.", ex);
            }
        }

        private void ProcessPayByText()
        {
            try
            {
                PayByText payByText = new PayByText();
                Utility utility = new Utility();
                List<PayByTextEntity> listActivePayByText = new List<PayByTextEntity>();

                listActivePayByText = payByText.GetActivePayByTextSetup();

                if (listActivePayByText.Count > 0)
                {
                    decimal minAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                    decimal maxAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]);
                    string textMessage = string.Empty;

                    foreach (var item in listActivePayByText)
                    {
                        decimal transAmount = Convert.ToDecimal(item.DueAmount);
                        if (transAmount < minAmount || transAmount > maxAmount)
                        {
                            textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextNotProcess, transAmount);
                            SendTextMessageToTextNumber(item.AcctNo, item.TextNumber, textMessage);
                            continue;
                        }

                        var paymentMethod = string.IsNullOrWhiteSpace(item.BankABA) ? "Card" : "ACH";

                        //Get fee
                        string fee = utility.GetFeeByPaymentMethodAndPaymentType(item.AcctNo, paymentMethod, "OneTime");

                        decimal totalAmountDue = item.DueAmount + Convert.ToDecimal(fee);

                        //get text message
                        textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.PayByTextPay, item.AcctNo, totalAmountDue.ToString(), item.DueDate.ToString("{0:MM/dd/yyyy}"));

                        ResponseMessage response = SendTextMessageToTextNumber(item.AcctNo, item.TextNumber, textMessage);

                        if (response.IsSuccess)
                        {
                            PayByTextMessageLogEntity log = new PayByTextMessageLogEntity(){
                            AcctNo = item.AcctNo,
                            TextNumber = item.TextNumber,
                            Message = textMessage,
                            Type = "SEND",
                            Keyword = "PAY"
                            };

                            //save pay by text message log
                            payByText.SavePayByTextMessageLog(log);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Pay By Text Processing.", ex);
            }
        }

        /// <summary>
        /// Send text messages for payment confirmation
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <param name="textMessage">Text Message to send</param>
        /// <returns></returns>
        public ResponseMessage SendPaymentConfirmationTextMessage(string accountNumber, string textMessage)
        {
            var response = new ResponseMessage();
            IMessageService messageService = new MessageService();
            IDataContext dataContext = new DataContext();
            try
            {
                var additionalNotifications = dataContext.GetAdditionalNotifications(accountNumber);

                if (additionalNotifications.Count > 0)
                {
                    foreach (var collection in additionalNotifications)
                    {
                        if (collection.IsPaymentConfirmationNotification == true)
                        {
                            ResponseMessage responseMessage = messageService.SendMessage(collection.TextNumber, textMessage);
                            response.IsSuccess = responseMessage.IsSuccess;
                            response.Message = response.Message;
                            if (response.IsSuccess == true)
                            {
                                dataContext.SaveTextMessage(accountNumber, collection.TextNumber, textMessage);
                            }
                        }
                    }
                }
                return response;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Send Text Messages
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <param name="textNumber">Text Number</param>
        /// <param name="textMessage">Message to send</param>
        /// <returns></returns>
        public ResponseMessage SendTextMessageToTextNumber(string accountNumber, string textNumber, string textMessage)
        {
            var response = new ResponseMessage();
            IMessageService messageService = new MessageService();
            IDataContext dataContext = new DataContext();
            try
            {
                ResponseMessage responseMessage = messageService.SendMessage(textNumber, textMessage);
                response.IsSuccess = responseMessage.IsSuccess;
                response.Message = response.Message;
                if (response.IsSuccess == true)
                {
                    dataContext.SaveTextMessage(accountNumber, textNumber, textMessage);
                }

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
