namespace MAF.SolutionByText
{
    using MAF.DAL;
    using MAF.ENTITY.SolutionByText;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class DataContext:IDataContext
    {
        private IDataCollection dataCollection = null;
        public DataContext()
        {
            this.dataCollection = new DataCollection();
        }

        /// <summary>
        /// Save Text Log
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="textNumber"></param>
        /// <param name="logType"></param>
        /// <param name="logEntry"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string SaveTextLog(string accountNumber, string textNumber, string logType, string logEntry, string username)
        {
            try
            {
                var accountID = new SqlParameter
                {
                    ParameterName = "AccountNumber",
                    Value = accountNumber
                };
                var number = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = textNumber
                };
                var textLogType = new SqlParameter
                {
                    ParameterName = "TextLogType",
                    Value = logType
                };
                var textLogEntry = new SqlParameter
                {
                    ParameterName = "TextLogEntry",
                    Value = logEntry
                };
                var textLogUserName = new SqlParameter
                {
                    ParameterName = "TextLogUserName",
                    Value = username
                };

                object[] parameters = new object[] { accountID, number, textLogType, textLogEntry, textLogUserName };
                return dataCollection.SaveTextLog(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Opt In Information of subscriber
        /// </summary>
        /// <param name="textNotificationEntity">Text Notification Model Object</param>
        /// <param name="optInIPAddress">Opt In IP Address</param>
        /// <param name="username">Opt Out IP Address</param>
        /// <returns></returns>
        public string OptInSubscriber(TextNotificationEntity textNotificationEntity, string optInIPAddress, string username)
        {
            try
            {
                var accountID = new SqlParameter
                {
                    ParameterName = "AccountNumber",
                    Value = textNotificationEntity.AccountNumber
                };
                var number = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = textNotificationEntity.TextNumber
                };
                var ipAddress = new SqlParameter
                {
                    ParameterName = "OptInIPAddress",
                    Value = optInIPAddress
                };
                var createdBy = new SqlParameter
                {
                    ParameterName = "CreatedBy",
                    Value = username
                };
                var isPaymentReminderNotification = new SqlParameter
                {
                    ParameterName = "IsPaymentReminderNotification",
                    Value = textNotificationEntity.IsPaymentReminder
                };
                var isPaymentConfirmationNotification = new SqlParameter
                {
                    ParameterName = "IsPaymentConfirmationNotification",
                    Value = textNotificationEntity.IsPaymentConfirmation
                };

                var isPayByTextNotification = new SqlParameter
                {
                    ParameterName = "IsPayByTextNotification",
                    Value = textNotificationEntity.IsPayByText
                };
                var isCommunicationByTextNotification = new SqlParameter
                {
                    ParameterName = "IsCommunicationByTextNotification",
                    Value = textNotificationEntity.IsCommunicationByText
                };

                object[] parameters = new object[] { accountID, number, ipAddress, 
                    createdBy, isPaymentReminderNotification, isPaymentConfirmationNotification, 
                    isPayByTextNotification, isCommunicationByTextNotification
                };
                return dataCollection.OptInSubscriber(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Opt Out Information of subscribers
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="optOutIPAddress"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string OptOutSubscriber(int subscriberId, string optOutIPAddress, string username)
        {
            try
            {
                var subscriber = new SqlParameter
                {
                    ParameterName = "SubscriberID",
                    Value = subscriberId
                };
                var ipAddress = new SqlParameter
                {
                    ParameterName = "OptOutIPAddress",
                    Value = optOutIPAddress
                };
                var modifiedBy = new SqlParameter
                {
                    ParameterName = "ModifiedBy",
                    Value = username
                };

                object[] parameters = new object[] { subscriber, ipAddress, modifiedBy };
                return dataCollection.OptOutSubscriber(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Subscriber details by account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public List<Subscriber> GetSubscriberByAccountNumber(string accountNumber)
        {
            try
            {
                var accountID = new SqlParameter
                {
                    ParameterName = "AccountNumber",
                    Value = accountNumber
                };

                object[] parameters = new object[] { accountID };
                return dataCollection.GetSubscriberByAccountNumber<Subscriber>(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Additional Notifications details by account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public List<Subscriber> GetAdditionalNotifications(string accountNumber)
        {
            try
            {
                var accountID = new SqlParameter
                {
                    ParameterName = "AccountNumber",
                    Value = accountNumber
                };

                object[] parameters = new object[] { accountID };
                return dataCollection.GetAdditionalNotificationsByAccountNumber<Subscriber>(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save additional notifications Information of subscribers
        /// </summary>
        /// <param name="subscriberId">Subscriber ID</param>
        /// <param name="isPaymentReminder">Payment Reminder</param>
        /// <param name="isPaymentConfirmation">Payment Confirmation</param>
        /// <param name="isPayByText">Pay By Text</param>
        /// <param name="isCommunicationByText">Communication By Text</param>
        /// <returns></returns>
        public string UpdateAdditionalNotifications(int subscriberId, bool isPaymentReminder, bool isPaymentConfirmation, bool isPayByText, bool isCommunicationByText)
        {
            try
            {
                var subscriber = new SqlParameter
                {
                    ParameterName = "SubscriberID",
                    Value = subscriberId
                };
                var isPaymentReminderNotification = new SqlParameter
                {
                    ParameterName = "IsPaymentReminderNotification",
                    Value = isPaymentReminder
                };
                var isPaymentConfirmationNotification = new SqlParameter
                {
                    ParameterName = "IsPaymentConfirmationNotification",
                    Value = isPaymentConfirmation
                };

                var isPayByTextNotification = new SqlParameter
                {
                    ParameterName = "IsPayByTextNotification",
                    Value = isPayByText
                };
                var isCommunicationByTextNotification = new SqlParameter
                {
                    ParameterName = "IsCommunicationByTextNotification",
                    Value = isCommunicationByText
                };
                object[] parameters = new object[] { subscriber, isPaymentReminderNotification, 
                    isPaymentConfirmationNotification, isPayByTextNotification
                    , isCommunicationByTextNotification
                };
                return dataCollection.UpdateAdditionalNotifications(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Callback Url Info
        /// </summary>
        /// <returns></returns>
        public string SaveCallbackUrlInfo(CallbackUrlInfoEntity objCallbackUrlInfo)
        {
            try
            {
                var title = new SqlParameter
                {
                    ParameterName = "Title",
                    Value = objCallbackUrlInfo.Title.OrDbNull()
                };
                var code = new SqlParameter
                {
                    ParameterName = "Code",
                    Value = objCallbackUrlInfo.Code.OrDbNull()
                };
                var shortCode = new SqlParameter
                {
                    ParameterName = "ShortCode",
                    Value = objCallbackUrlInfo.ShortCode.OrDbNull()
                };
                var message = new SqlParameter
                {
                    ParameterName = "Message",
                    Value = objCallbackUrlInfo.Message.OrDbNull()
                };
                var phone = new SqlParameter
                {
                    ParameterName = "Phone",
                    Value = objCallbackUrlInfo.Phone.OrDbNull()
                };
                var carrier = new SqlParameter
                {
                    ParameterName = "Carrier",
                    Value = objCallbackUrlInfo.Carrier.OrDbNull()
                };
                var keyword = new SqlParameter
                {
                    ParameterName = "Keyword",
                    Value = objCallbackUrlInfo.Keyword.OrDbNull()
                };
                var group = new SqlParameter
                {
                    ParameterName = "Group",
                    Value = objCallbackUrlInfo.Group.OrDbNull()
                };
                var note = new SqlParameter
                {
                    ParameterName = "Note",
                    Value = objCallbackUrlInfo.Note.OrDbNull()
                };
                var uniqueId = new SqlParameter
                {
                    ParameterName = "UniqueId",
                    Value = objCallbackUrlInfo.UniqueId.OrDbNull()
                };
                var defaultKeyword = new SqlParameter
                {
                    ParameterName = "Default_Keyword",
                    Value = objCallbackUrlInfo.DefaultKeyword.OrDbNull()
                };

                object[] parameters = new object[] { 
                    title, code, shortCode, message, phone, carrier, keyword, group, note, uniqueId, defaultKeyword
                };
                return dataCollection.SaveCallbackUrlInfo(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Status Url Info
        /// </summary>
        /// <returns></returns>
        public string SaveStatusUrlInfo(StatusUrlInfoEntity objStatusUrlInfo)
        {
            try
            {
                var code = new SqlParameter
                {
                    ParameterName = "Code",
                    Value = objStatusUrlInfo.Code.OrDbNull()
                };
                var message = new SqlParameter
                {
                    ParameterName = "Message",
                    Value = objStatusUrlInfo.Message.OrDbNull()
                };
                var sendTo = new SqlParameter
                {
                    ParameterName = "SendTo",
                    Value = objStatusUrlInfo.SendTo.OrDbNull()
                };
                var orgCode = new SqlParameter
                {
                    ParameterName = "OrgCode",
                    Value = objStatusUrlInfo.OrgCode.OrDbNull()
                };
                var status = new SqlParameter
                {
                    ParameterName = "Status",
                    Value = objStatusUrlInfo.Status.OrDbNull()
                };
                var ticket = new SqlParameter
                {
                    ParameterName = "Ticket",
                    Value = objStatusUrlInfo.Ticket.OrDbNull()
                };
                var note = new SqlParameter
                {
                    ParameterName = "Note",
                    Value = objStatusUrlInfo.Note.OrDbNull()
                };
                var uniqueId = new SqlParameter
                {
                    ParameterName = "UniqueId",
                    Value = objStatusUrlInfo.UniqueId.OrDbNull()
                };
                var messageCategory = new SqlParameter
                {
                    ParameterName = "MessageCategory",
                    Value = objStatusUrlInfo.MessageCategory.OrDbNull()
                };
                var messageSubCategory = new SqlParameter
                {
                    ParameterName = "MessageSubCategory",
                    Value = objStatusUrlInfo.MessageSubCategory.OrDbNull()
                };

                object[] parameters = new object[] { 
                    code, message, sendTo, orgCode,  status, ticket, note, uniqueId, messageCategory, messageSubCategory
                };
                return dataCollection.SaveStatusUrlInfo(parameters);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Save Outbound Text Messages 
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        /// <param name="createdName"></param>
        /// <returns></returns>
        public string SaveTextMessage(string accountNumber, string textNumber, string message)
        {
            try
            {
                var accountNo = new SqlParameter
                {
                    ParameterName = "AccountNumber",
                    Value = accountNumber.OrDbNull()
                };
                var textNo = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = textNumber.OrDbNull()
                };
                var textMessage = new SqlParameter
                {
                    ParameterName = "Textmessage",
                    Value = message.OrDbNull()
                };
                object[] parameters = new object[] { accountNo, textNo, textMessage };
                return dataCollection.SaveOutboundTextMessage(parameters);
            }
            catch
            {
                throw;
            }
        }
    }

    public static class SqlParameterHelper
    {
        public static object OrDbNull(this string s)
        {
            return string.IsNullOrEmpty(s) ? DBNull.Value : (object)s;
        }
    }
}
