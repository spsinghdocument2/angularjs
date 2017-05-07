namespace MAF.SolutionByText
{
    using MAF.SolutionByText.Resources;
    using MAF.SolutionByText.SubscriberWSServiceReference;
    using System.Configuration;

    public class SubscriberService : ISubscriberService
    {
        /// <summary>
        /// Method to unsuscribe the subscriber
        /// </summary>
        /// <param name="phoneNumber">phone number to unsubscribe</param>
        /// <returns></returns>
        public ResponseMessage UnSubscribe(string phoneNumber)
        {
            try
            {
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                string orgCode = ConfigurationManager.AppSettings["SBTOrganizationCode"];

                //Get security token
                ILoginService loginService = new LoginService();
                securityToken = loginService.GetSecurityToken();

                SubscriberClient subscriberService = new SubscriberClient(Common.SubscriberServiceEndpoint);

                SubscriberDetails subscriber = new SubscriberDetails();
                subscriber.MobilePhone = phoneNumber; 
                subscriber.OrgCode = orgCode;

                WSUnsubscriberResponse wSUnsubscriberResponse = subscriberService.UnSubscribe(securityToken, subscriber);

                responseMessage.IsSuccess = wSUnsubscriberResponse.Result;
                responseMessage.Message = wSUnsubscriberResponse.Message;
                Logger.WriteTraceLog(wSUnsubscriberResponse.Message);
                return responseMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update subscriber unique id with account number
        /// </summary>
        /// <param name="phoneNumber">Phone Number of subscriber</param>
        /// <param name="accountNumber">Account Number of subscriber</param>
        /// <returns></returns>
        public ResponseMessage UpdateSubscriber(string phoneNumber, string accountNumber)
        {
            try
            {
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                string orgCode = ConfigurationManager.AppSettings["SBTOrganizationCode"];

                //Get security token
                ILoginService loginService = new LoginService();
                securityToken = loginService.GetSecurityToken();

                SubscriberClient subscriberService = new SubscriberClient(Common.SubscriberServiceEndpoint);

                SubscriberInfo subscriber = new SubscriberInfo();
                subscriber.MobilePhone = phoneNumber;
                subscriber.UniqueID = accountNumber;

                WSSubscriberResponse wSSubscriberResponse = subscriberService.UpdateSubscriber(securityToken, subscriber);
                responseMessage.IsSuccess = wSSubscriberResponse.Result;
                responseMessage.Message = wSSubscriberResponse.Message;
                Logger.WriteTraceLog(wSSubscriberResponse.Message);
                return responseMessage;
            }
            catch
            {
                
                throw;
            }
        }
    }
}
