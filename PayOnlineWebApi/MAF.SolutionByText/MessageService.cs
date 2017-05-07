namespace MAF.SolutionByText
{
    using MAF.DAL;
    using MAF.ENTITY.SolutionByText;
    using MAF.SolutionByText.MessageWSServiceReference;
    using MAF.SolutionByText.Resources;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
  

    public class MessageService : IMessageService
    {

        /// <summary>
        /// Method to send request for VBT
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        /// <returns></returns>
        public ResponseMessage RequestVBT(string phoneNumber)
        {
            try
            {
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                string orgCode = ConfigurationManager.AppSettings["SBTOrganizationCode"];

                //Get security token
                ILoginService loginService = new LoginService();
                securityToken = loginService.GetSecurityToken();

                //Send request to RequireVBT
                MessageClient messageResult = new MessageClient(Common.MessageServiceEndpoint);

                WSVerificationResponse wSVerificationResponse = messageResult.RequestVBT(securityToken, orgCode, phoneNumber);

                responseMessage.IsSuccess = wSVerificationResponse.Result;

                Logger.WriteTraceLog(wSVerificationResponse.Message);
                if (!wSVerificationResponse.Result)
                {
                    responseMessage.Message = wSVerificationResponse.Message;
                }
                else
                {
                    responseMessage.Message = wSVerificationResponse.Pin;
                }
                return responseMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// method to confirm vbt request pin
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="pin">pin</param>
        /// <returns></returns>
        public ResponseMessage ConfirmVBT(string phoneNumber, string pin)
        {
            try
            {
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                string orgCode = ConfigurationManager.AppSettings["SBTOrganizationCode"];

                //Get security token 
                ILoginService loginService = new LoginService();
                securityToken = loginService.GetSecurityToken();

                //Send request to ConfirmVBT
                MessageClient messageResult = new MessageClient(Common.MessageServiceEndpoint);
                WSVerificationResponse wSVerificationResponse = messageResult.ConfirmVBT(securityToken, orgCode, phoneNumber, pin);

                responseMessage.IsSuccess = wSVerificationResponse.Result;
                responseMessage.Message = wSVerificationResponse.Message;
                Logger.WriteTraceLog(wSVerificationResponse.Message);
                return responseMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// send text message
        /// </summary>
        /// <param name="TextNumber">TextNumber</param>
        /// /// <param name="message">message</param>
        /// <returns></returns>
        public ResponseMessage SendMessage(string textNumber, string message)
        {
            try
            {              
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                string orgCode = ConfigurationManager.AppSettings["SBTTextMessagesOrganizationCode"];              
                string statusUrl = ConfigurationManager.AppSettings["SBTStatusUrl"];
                //Get security token 
                ILoginService loginService = new LoginService();
                securityToken = loginService.GetSecurityToken();
  
                List<WSRecipient> listRecipient = new List<WSRecipient>();
                WSRecipient wsRecipient = new WSRecipient();
                wsRecipient.SendTo = textNumber;
                listRecipient.Add(wsRecipient);
                //Send request to ConfirmVBT
                 MessageClient messageResult = new MessageClient(Common.MessageServiceEndpoint);
                 WSMessageResponse wSMessageResponse = messageResult.SendMessage(securityToken, message, listRecipient.ToArray(), orgCode, string.Empty, statusUrl);
                 responseMessage.IsSuccess = wSMessageResponse.Result;
                 responseMessage.Message = wSMessageResponse.Message;
                 Logger.WriteTraceLog(wSMessageResponse.Message);

                
                  return responseMessage;
                 
            }
            catch
            {
                throw;
            }
        }
   

    
    }
}
