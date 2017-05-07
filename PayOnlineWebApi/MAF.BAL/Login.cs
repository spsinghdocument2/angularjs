using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class Login : ILogin
    {
        IDataCollection dataCollection;
        string accountNumber = string.Empty;
        public Login()
        {
            this.dataCollection = new DataCollection();
        }

        #region  Login Account
        public LoginEntity UserLogin(LoginEntity loginEntity)
        {
            string message = string.Empty;
            try
            {
                var accountNumber = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = loginEntity.AccountNumber.OrDbNull()
                };
                var username = new SqlParameter
                {
                    ParameterName = "Username",
                    Value = loginEntity.Email.OrDbNull()
                };
                var password = new SqlParameter
                {
                    ParameterName = "Password",
                    Value = loginEntity.Password.OrDbNull()
                };

                object[] parameters = new object[] { accountNumber, username, password };
                return dataCollection.Login<LoginEntity>(parameters).FirstOrDefault();
            }
            catch
            {
                throw;//message = MAF.BAL.ResourceFile.Common.LoginException;// "System Maintenance, please try again later.";
            }
            
        }
        #endregion
    }
}
