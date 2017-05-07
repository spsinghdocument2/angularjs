namespace MAF.WEBAPI
{
    using MAF.ENTITY;
    using MAF.SolutionByText;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;

    public class Helper
    {
        private static readonly Helper instance = new Helper();
        private Helper()
        { }

        public static Helper Instance
        {
            get
            {
                return instance;
            }
        }

        #region Get Client IP address
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        public string GetClientIp(HttpRequestMessage request = null)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }
        #endregion

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
        /// <param name="textMessage">Message to send</param>
        /// <returns></returns>
        public ResponseMessage SendTextMessage(string accountNumber, string textMessage)
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
                        ResponseMessage responseMessage = messageService.SendMessage(collection.TextNumber, textMessage);
                        response.IsSuccess = responseMessage.IsSuccess;
                        response.Message = response.Message;
                        if (response.IsSuccess == true)
                        {
                            dataContext.SaveTextMessage(accountNumber, collection.TextNumber, textMessage);
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