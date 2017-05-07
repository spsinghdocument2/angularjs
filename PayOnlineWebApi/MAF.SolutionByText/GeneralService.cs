namespace MAF.SolutionByText
{
    using MAF.SolutionByText.GeneralWSServiceReference;
    using MAF.SolutionByText.Resources;
    using System.Collections.Generic;
    using System.Configuration;

    public class GeneralService : IGeneralService
    {
        /// <summary>
        /// Method to do carrier lookup(validate) the number
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        /// <returns></returns>
        public ResponseMessage GetCarrierLookup(string phoneNumber)
        {
            try
            {
                string securityToken = string.Empty;
                ResponseMessage responseMessage = new ResponseMessage();
                ILoginService loginService = new LoginService();

                //Get security token 
                securityToken = loginService.GetSecurityToken();

                //SBT Service for carrier lookup
                GroupClient generalResult = new GroupClient(Common.GeneralServiceEndpoint);

                List<string> phone = new List<string>();
                phone.Add(phoneNumber);

                WSCarrierLookupResponse wSCarrierLookupResponse = generalResult.GetCarrierLookup(securityToken, phone.ToArray(), ConfigurationManager.AppSettings["SBTOrganizationCode"]);

                if (!wSCarrierLookupResponse.Result)
                {
                responseMessage.IsSuccess = wSCarrierLookupResponse.Result;
                responseMessage.Message = wSCarrierLookupResponse.Message;
                }
                else
                {
                    WSCarrierPhoneRow[] wSCarrierPhoneRow = wSCarrierLookupResponse.Response;
                    if (!wSCarrierPhoneRow[0].International && !wSCarrierPhoneRow[0].Invalid && !wSCarrierPhoneRow[0].Landline)
                    {
                        responseMessage.IsSuccess = wSCarrierLookupResponse.Result;
                        responseMessage.Message = wSCarrierLookupResponse.Message;
                    }
                    else
                    {
                        responseMessage.IsSuccess = false;
                        responseMessage.Message = "Text number "+ phoneNumber + " is invalid!";
                    }
                }

                Logger.WriteTraceLog(wSCarrierLookupResponse.Message);

                return responseMessage;
            }
            catch
            {
                throw;
            }
        }
    }
}
