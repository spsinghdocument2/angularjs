using MAF.BAL;
using MAF.ENTITY;
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
    [RoutePrefix("api/TextNotification")]
    public class TextNotificationController : ApiController
    {
        //Intialize interface members
        private readonly IDataContext dataContext;
        private readonly IGeneralService generalService;
        private readonly IMessageService messageService;
        private readonly ISubscriberService subscriberService;
        private readonly IUserProfile userProfile;
        private readonly IPayByText payByText;
        public SendMail sendMail;

        public TextNotificationController(IDataContext dataContext, IGeneralService generalService, IMessageService messageService, ISubscriberService subscriberService, IUserProfile userProfile, IPayByText payByText)
        {
            this.userProfile = userProfile;
            this.dataContext = dataContext;
            this.generalService = generalService;
            this.messageService = messageService;
            this.subscriberService = subscriberService;
            this.payByText = payByText;
        }

        /// <summary>
        /// This method is to get opted-in text number details with additional notifications
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserDetails/{accountNumber}")]
        public HttpResponseMessage GetUserDetails(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    //Get user details based on account number
                    UserProfileEntity userProfileEntity = new UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(accountNumber);

                    //Get subscriber based on account number
                    List<TextNotificationEntity> subscribers = new List<TextNotificationEntity>();
                    subscribers = dataContext.GetSubscriberByAccountNumber(accountNumber).Select(s => new TextNotificationEntity
                    {
                        TextNumber = s.TextNumber,
                        IsPayByText = s.IsPayByTextNotification,
                        IsPaymentReminder = s.IsPaymentReminderNotification,
                        IsPaymentConfirmation = s.IsPaymentConfirmationNotification,
                        IsCommunicationByText = s.IsCommunicationByTextNotification
                    }).ToList();

                    foreach (var subscriber in subscribers.Where(s => s.TextNumber.Equals(userProfileEntity.TextNumber)))
                    {
                        subscriber.IsPrimaryTextNumber = true;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, subscribers);
                }
            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in GetUserDetails.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// This action method to get text number from database which are not already opted in for text by notifications
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTextNumberDetails/{accountNumber}")]
        public HttpResponseMessage GetTextNumberDetails(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    //Get subscriber based on account number

                    List<string> listTextNumbers = new List<string>();

                    //Get user details based on account number
                    UserProfileEntity userProfileEntity = new UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(accountNumber);
                    //Get subscriber based on account number
                    List<Subscriber> subscriber = new List<Subscriber>();
                    subscriber = dataContext.GetSubscriberByAccountNumber(accountNumber);

                    if (!string.IsNullOrWhiteSpace(userProfileEntity.TextNumber))
                    {
                        var qry = subscriber
                         .Where(s => s.TextNumber == userProfileEntity.TextNumber).ToList();

                        if (qry.Count == 0)
                            listTextNumbers.Add(userProfileEntity.TextNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(userProfileEntity.TextNumber2))
                    {
                        var qry = subscriber
                           .Where(s => s.TextNumber == userProfileEntity.TextNumber2).ToList();

                        if (qry.Count == 0)
                            listTextNumbers.Add(userProfileEntity.TextNumber2);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, listTextNumbers);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in GetTextNumberDetails.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }
        
        /// <summary>
        /// Action method to validate user cell/text number and to send verification PIN
        /// </summary>
        /// <param name="textNotificationEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestVerification")]
        public HttpResponseMessage RequestVerification(TextNotificationEntity textNotificationEntity)
        {
            try
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                if (string.IsNullOrWhiteSpace(textNotificationEntity.AccountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    //Get user details based on account number
                    MAF.ENTITY.UserProfileEntity userProfileEntity = new ENTITY.UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(textNotificationEntity.AccountNumber);

                    if (userProfileEntity == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "User data not found!");

                    //Get subscriber based on account number
                    List<Subscriber> subscribers = new List<Subscriber>();
                    subscribers = dataContext.GetSubscriberByAccountNumber(textNotificationEntity.AccountNumber);
                    if(subscribers.Count >= 2)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Maximum 2 Text Number is allowed for Notifications By Text.");

                    responseMessage = RequestVerificationPin(textNotificationEntity.AccountNumber, textNotificationEntity.TextNumber, userProfileEntity.Email, userProfileEntity.FullName);

                    //update text number
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        if (!textNotificationEntity.TextNumber.Equals(userProfileEntity.TextNumber2) && !textNotificationEntity.TextNumber.Equals(userProfileEntity.TextNumber))
                        {
                            if (string.IsNullOrEmpty(userProfileEntity.TextNumber) && string.IsNullOrEmpty(userProfileEntity.TextNumber2))
                            {
                                userProfileEntity.TextNumber = textNotificationEntity.TextNumber;
                            }
                            else if (string.IsNullOrEmpty(userProfileEntity.TextNumber) && !string.IsNullOrEmpty(userProfileEntity.TextNumber2))
                            {
                                userProfileEntity.TextNumber = textNotificationEntity.TextNumber;
                            }
                            else if (!string.IsNullOrEmpty(userProfileEntity.TextNumber) && string.IsNullOrEmpty(userProfileEntity.TextNumber2))
                            {
                                userProfileEntity.TextNumber2 = textNotificationEntity.TextNumber;
                            }
                            else
                            {
                                //Get subscriber based on account number
                                Subscriber subscriber = new Subscriber();
                                subscriber = dataContext.GetSubscriberByAccountNumber(textNotificationEntity.AccountNumber).Where(s => s.TextNumber == userProfileEntity.TextNumber).FirstOrDefault();
                                if (subscriber == null)
                                    userProfileEntity.TextNumber = textNotificationEntity.TextNumber;
                                else
                                    userProfileEntity.TextNumber2 = textNotificationEntity.TextNumber;
                            }
                            if (string.IsNullOrEmpty(userProfileEntity.Image))
                                userProfileEntity.Image = string.Empty;
                            userProfile.UpdateUserProfile(userProfileEntity.Email, userProfileEntity.CellNumber, userProfileEntity.TextNumber, userProfileEntity.TextNumber2, userProfileEntity.Address1, userProfileEntity.AccountNumber, userProfileEntity.Image, userProfileEntity.Address2, userProfileEntity.City, userProfileEntity.State, userProfileEntity.Zip);
                        }
                    }
                    return responseMessage;
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Request Verification.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Method to verify text number and request verification pin
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="textNumber"></param>
        /// <param name="email"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private HttpResponseMessage RequestVerificationPin(string accountNumber, string textNumber, string email, string fullName)
        {
            try
            {
                sendMail = new SendMail();
                string message = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();

                //Do carrier lookup to validate phone number
                responseMessage = generalService.GetCarrierLookup(textNumber);

                if (!responseMessage.IsSuccess)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);
                }
                dataContext.SaveTextLog(accountNumber, textNumber, "CarrierLookup", "Carrier Lookup is successful.", fullName);

                //Send verification pin
                responseMessage = messageService.RequestVBT(textNumber);

                if (!responseMessage.IsSuccess)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);

                dataContext.SaveTextLog(accountNumber, textNumber, "RequestPIN", "Verification PIN " + responseMessage.Message + " sent.", fullName);

                //Uncomment below line to use actual email address
                sendMail.OptInMail(email, textNumber);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Action method to confirm verification pin, save subscriber and update subscriber with account number
        /// </summary>
        /// <param name="textNotificationEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmVerification")]
        public HttpResponseMessage ConfirmVerification(TextNotificationEntity textNotificationEntity)
        {
            try
            {
                sendMail = new SendMail();
                string message = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                if (string.IsNullOrWhiteSpace(textNotificationEntity.AccountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    string textNumber = textNotificationEntity.TextNumber;

                    //Get user details based on account number
                    MAF.ENTITY.UserProfileEntity userProfileEntity = new ENTITY.UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(textNotificationEntity.AccountNumber);

                    if (userProfileEntity == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "User data not found!");

                    //Get subscriber based on account number
                    List<Subscriber> subscribers = new List<Subscriber>();
                    subscribers = dataContext.GetSubscriberByAccountNumber(textNotificationEntity.AccountNumber);
                    if (subscribers.Count > 0)
                    {
                        var subscriber = subscribers.Where(s => s.TextNumber.Equals(textNotificationEntity.TextNumber)).ToList();

                        if (subscriber.Count > 0)
                            return Request.CreateResponse(HttpStatusCode.NotImplemented, "The Text Number " + textNotificationEntity.TextNumber + " is already opted in.");
                        if (subscribers.Count >= 2)
                            return Request.CreateResponse(HttpStatusCode.NotImplemented, "Maximum 2 Text NUmber is allowed for Notifications By Text.");
                    }

                    //Confirm verification pin
                    responseMessage = messageService.ConfirmVBT(textNotificationEntity.TextNumber, textNotificationEntity.Pin);

                    if (!responseMessage.IsSuccess)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);

                    dataContext.SaveTextLog(textNotificationEntity.AccountNumber, textNumber, "ConfirmPIN", "Verified PIN " + responseMessage.Message + ".", userProfileEntity.FullName);

                    //Save subscriber detail in db
                    string result = dataContext.OptInSubscriber(textNotificationEntity, GetClientIp(Request), userProfileEntity.FullName);
                    dataContext.SaveTextLog(textNotificationEntity.AccountNumber, textNumber, "Subscribe", "Opted-In for Text Notification.", userProfileEntity.FullName);

                    //update subscriber
                    responseMessage = subscriberService.UpdateSubscriber(textNumber, textNotificationEntity.AccountNumber);
                    if (responseMessage.IsSuccess)
                    {
                        //Send text messages
                        if (textNotificationEntity.IsPaymentReminder)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnablePaymentReminder);
                        if (textNotificationEntity.IsPaymentConfirmation)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnablePaymentConfirmation);
                        if (textNotificationEntity.IsCommunicationByText)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnableChatByText);

                        dataContext.SaveTextLog(textNotificationEntity.AccountNumber, textNumber, "UpdateSubscriber", "Subscriber UniqueID updated with account number.", userProfileEntity.FullName);
                        sendMail.VerifyPinMail(userProfileEntity.Email);//userProfileEntity.Email
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Confirm Verification.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// UnSubscriber from text notification
        /// </summary>
        /// <param name="textNotificationEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnSubscribe")]
        public HttpResponseMessage UnSubscribe(TextNotificationEntity textNotificationEntity)
        {
            try
            {
                sendMail = new SendMail();
                string message = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                if (string.IsNullOrWhiteSpace(textNotificationEntity.AccountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    string textNumber = textNotificationEntity.TextNumber;
                    MAF.ENTITY.UserProfileEntity userProfileEntity = new ENTITY.UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(textNotificationEntity.AccountNumber);

                    //Get subscriber based on account number
                    Subscriber subscriber = new Subscriber();
                    subscriber = dataContext.GetSubscriberByAccountNumber(textNotificationEntity.AccountNumber).Where(s=> s.TextNumber == textNumber).FirstOrDefault();

                    if (subscriber == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Subscriber not active.");

                    if (subscriber.IsActive)
                    {
                        textNumber = subscriber.TextNumber;

                        //Send text message
                        var response = Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.NotificationsByTextOptOut);

                        //Delete pay by text setup
                        if (subscriber.IsPayByTextNotification)
                            DeletePayByTextSetup(textNotificationEntity.AccountNumber, textNumber);

                        //unsubcribe from sbt server
                        responseMessage = subscriberService.UnSubscribe(subscriber.TextNumber);

                        if (!responseMessage.IsSuccess)
                            return Request.CreateResponse(HttpStatusCode.BadRequest, responseMessage.Message);

                        //Save subscriber detail in db
                        string result = dataContext.OptOutSubscriber(subscriber.SubscriberID, GetClientIp(Request), subscriber.CreatedBy);

                        dataContext.SaveTextLog(textNotificationEntity.AccountNumber, textNumber, "UnSubscribe", "Opted-Out from Text Notification.", subscriber.CreatedBy);
                        sendMail.OptOutMail(userProfileEntity.Email, textNumber);//userProfileEntity.Email
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in UnSubscribe.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Get subscriber details based on account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubscription/{accountNumber}")]
        public HttpResponseMessage GetSubscriber(string accountNumber)
        {
            try
            {
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    //Get subscriber based on account number
                    List<Subscriber> subscribers = new List<Subscriber>();
                    subscribers = dataContext.GetSubscriberByAccountNumber(accountNumber);

                    return Request.CreateResponse(HttpStatusCode.OK, subscribers);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in GetSubscriber.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Update additional notification for subscriber
        /// </summary>
        /// <param name="textNotificationEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAdditionalNotifications")]
        public HttpResponseMessage UpdateAdditionalNotifications(TextNotificationEntity textNotificationEntity)
        {
            try
            {
                sendMail = new SendMail();
                string message = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                if (string.IsNullOrWhiteSpace(textNotificationEntity.AccountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    string textNumber = textNotificationEntity.TextNumber;
                    MAF.ENTITY.UserProfileEntity userProfileEntity = new ENTITY.UserProfileEntity();
                    userProfileEntity = userProfile.GetUserProfile(textNotificationEntity.AccountNumber);

                    //Get subscriber based on account number
                    Subscriber subscriber = new Subscriber();
                    subscriber = dataContext.GetSubscriberByAccountNumber(textNotificationEntity.AccountNumber).Where(s => s.TextNumber == textNumber).FirstOrDefault();

                    if (subscriber == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Subscriber not active.");

                    if (subscriber.IsActive)
                    {
                        textNumber = subscriber.TextNumber;

                        //Update subscriber on solutions by text
                        if(!subscriber.IsApproved)
                            responseMessage = subscriberService.UpdateSubscriber(textNumber, textNotificationEntity.AccountNumber);

                        //Save subscriber detail in db
                        string result = dataContext.UpdateAdditionalNotifications(subscriber.SubscriberID, textNotificationEntity.IsPaymentReminder, textNotificationEntity.IsPaymentConfirmation, textNotificationEntity.IsPayByText, textNotificationEntity.IsCommunicationByText);

                        dataContext.SaveTextLog(textNotificationEntity.AccountNumber, textNumber, "AdditionalNotifications", "Updated additional notifications.", subscriber.CreatedBy);

                        //Send text messages for Additional Notifications
                        if (!subscriber.IsPaymentReminderNotification && textNotificationEntity.IsPaymentReminder)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnablePaymentReminder);
                        
                        if (!subscriber.IsPaymentConfirmationNotification && textNotificationEntity.IsPaymentConfirmation)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnablePaymentConfirmation);
                        
                        if (!subscriber.IsCommunicationByTextNotification && textNotificationEntity.IsCommunicationByText)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.EnableChatByText);

                        if (subscriber.IsPaymentReminderNotification && !textNotificationEntity.IsPaymentReminder)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.DisablePaymentReminder);

                        if (subscriber.IsPaymentConfirmationNotification && !textNotificationEntity.IsPaymentConfirmation)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.DisablePaymentConfirmation);

                        if (subscriber.IsCommunicationByTextNotification && !textNotificationEntity.IsCommunicationByText)
                            Helper.Instance.SendTextMessageToTextNumber(textNotificationEntity.AccountNumber, textNumber, MAF.BAL.ResourceFile.TextMessages.DisableChatByText);


                        //Delete pay by text setup
                        if (subscriber.IsPayByTextNotification && !textNotificationEntity.IsPayByText)
                        {
                            DeletePayByTextSetup(textNotificationEntity.AccountNumber, textNumber);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in UpdateAdditionalNotifications.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Get additional notification details based on account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAdditionalNotifications/{accountNumber}")]
        public HttpResponseMessage GetAdditionalNotifications(string accountNumber)
        {
            try
            {
                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Number cannot be blank.");
                }
                else
                {
                    //Get subscriber based on account number
                    List<Subscriber> subscribers = new List<Subscriber>();
                    subscribers = dataContext.GetAdditionalNotifications(accountNumber);

                    return Request.CreateResponse(HttpStatusCode.OK, subscribers);
                }
            }
            catch(Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in GetAdditionalNotifications.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }

        /// <summary>
        /// Method to delete pay by text and send message
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <param name="textNumber">Text Number</param>
        private void DeletePayByTextSetup(string accountNumber, string textNumber)
        {
            try
            {
                payByText.DeletePayByTextSetup(accountNumber);

                var textMessage = string.Format(MAF.BAL.ResourceFile.TextMessages.DeletePayByText, accountNumber);
                var response = Helper.Instance.SendTextMessageToTextNumber(accountNumber, textNumber, textMessage);
            }
            catch
            {
                throw;
            }
        }

        #region Get Client IP address
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            
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
    }
}