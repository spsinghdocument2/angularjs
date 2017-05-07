namespace MAF.SolutionByText
{
    using MAF.DAL;
    using MAF.SolutionByText.LoginWSServiceReference;
    using MAF.SolutionByText.Resources;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// Contains members for login to SolutionByText api and to obtain SecurityToken
    /// </summary>
    public class LoginService : ILoginService
    {
        private IDataCollection dataCollection = null;
        public LoginService()
        {
            this.dataCollection = new DataCollection();
        }

        /// <summary>
        /// Method to authenticate SolutionByText api
        /// </summary>
        /// <returns></returns>
        public ResponseMessage Authenticate()
        {
            try
            {
                ResponseMessage responseMessage = new ResponseMessage();
                bool isAuthenticated = false;
                string securityToken = string.Empty;

                // Get api key based on level
                string apiKey = ConfigurationManager.AppSettings["SBTApiKey"];
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    //Call SBT LoginService to authenticate
                    LoginAPIClient login = new LoginAPIClient(Common.LoginAPIServiceEndpoint);

                    // Create loginRequestDetails
                    LoginRequestDetails loginRequestDetails = new LoginRequestDetails();
                    loginRequestDetails.APIKey = apiKey;

                    WSLoginResponse wSLoginResponse = login.AuthenticateAPIKey(loginRequestDetails);

                    Logger.WriteTraceLog(wSLoginResponse.Message);
                    //process response
                    if (!wSLoginResponse.Result)
                    {
                        isAuthenticated = false;
                        responseMessage.Message = wSLoginResponse.Message;
                    }
                    else
                    {
                        //handle success
                        isAuthenticated = true;
                        securityToken = wSLoginResponse.SecurityToken;

                        responseMessage.Message = securityToken;
                        //Save token in database
                        string result = SaveSecurityToken(securityToken, ConfigurationManager.AppSettings["SBTSecurityTokenExpireInMinutes"]);
                    }
                }
                else
                {
                    isAuthenticated = false;
                    Logger.WriteTraceLog("No api key specified.");
                }
                responseMessage.IsSuccess = isAuthenticated;
                return responseMessage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to save security token
        /// </summary>
        /// <param name="token">token to save</param>
        /// <param name="tokenExpireInMinutes">Duration in minutes after which token expires</param>
        /// <returns></returns>
        public string SaveSecurityToken(string token, string tokenExpireInMinutes)
        {
            try
            {
                var securityToken = new SqlParameter
                {
                    ParameterName = "SecurityToken",
                    Value = token
                };
                var tokenExpireDuration = new SqlParameter
                {
                    ParameterName = "TokenExpireInMinutes",
                    Value = tokenExpireInMinutes
                };
                object[] parameters = new object[] { securityToken, tokenExpireDuration };
                return dataCollection.SaveSecurityToken(parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get security token
        /// </summary>
        /// <returns>Security token</returns>
        public string GetSecurityToken()
        {
            try
            {
                string securityToken = string.Empty;
                bool isProduction = Convert.ToBoolean(ConfigurationManager.AppSettings["SBTIsProduction"]);

                //Get security token from database
                securityToken = dataCollection.GetSecurityToken<string>().FirstOrDefault();

                //check security token, if null then authenticate again
                if (string.IsNullOrWhiteSpace(securityToken))
                {
                    ResponseMessage responseMessage = Authenticate();

                    securityToken = responseMessage.IsSuccess ? responseMessage.Message : string.Empty;
                }
                return securityToken;
            }
            catch
            {
                throw;
            }
        }
    }
}
