namespace MAF.BAL
{
    using MAF.DAL;
    using MAF.ENTITY;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class AutoPayAgreement : IAutoPayAgreement
    {
        private IDataCollection dataCollection = null;
        public AutoPayAgreement()
        {
            this.dataCollection = new DataCollection();
        }

        /// <summary>
        /// Get autopay payment method
        /// </summary>
        /// <param name="accountNumber">account number</param>
        /// <returns>payment method</returns>
        public string GetAutoPayPaymentMethod(string accountNumber)
        {
            try
            {
                var acctNo = new SqlParameter
                {
                    ParameterName = "accountNumber",
                    Value = accountNumber.OrDbNull()
                };
                object[] parameters = new object[] { acctNo };

                return dataCollection.GetAutoPayPaymentMethod<string>(parameters).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        

        /// <summary>
        /// Method to update autopay web confirmation number
        /// </summary>
        /// <param name="accountNumber">AccountNumber</param>
        /// <param name="confirmationNumber">Confirmation Number</param>
        /// <returns>Status</returns>
        public string UpdateAutoPayConfirmationNumber(string accountNumber, string confirmationNumber)
        {
            try
            {
                var acctNo = new SqlParameter
                {
                    ParameterName = "accountNumber",
                    Value = accountNumber.OrDbNull()
                };
                var webConfirmationNumber = new SqlParameter
                {
                    ParameterName = "confirmationNumber",
                    Value = confirmationNumber.OrDbNull()
                };
                object[] parameters = new object[] { acctNo, webConfirmationNumber };

                return dataCollection.UpdateAutoPayWebConfirmationNumber(parameters);
            }
            catch
            {
                throw;
            }
        }
    }
}
