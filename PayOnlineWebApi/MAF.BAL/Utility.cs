namespace MAF.BAL
{
    using MAF.DAL;
    using MAF.ENTITY;
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Xml.Linq;

    public class Utility : IUtility
    {
        private IDataCollection dataCollection = null;
        public Utility()
        {
            this.dataCollection = new DataCollection();
        }

        /// <summary>
        /// Method to get the fee for transaction
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <param name="paymentMethod">Payment Method (Either ACH or Card)</param>
        /// <param name="paymentType">Payment Type (Either OneTime or Recurring)</param>
        /// <returns>Fee</returns>
        public string GetFeeByPaymentMethodAndPaymentType(string accountNumber, string paymentMethod, string paymentType)
        {
            try
            {
                string dealerAccountNumber = string.Empty, state = string.Empty, fee = string.Empty;
                var acctNumber = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };

                object[] parameters = new object[] { acctNumber };

                //Get account details
                var accountDetail = dataCollection.GetAccountDetails<AccountEntity>(parameters).FirstOrDefault();

                if (accountDetail != null)
                {

                    dealerAccountNumber = accountDetail.DlrAcctNo;
                    state = accountDetail.State;

                    //Get path of xml file for fee
                    var filePath = System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["FeeXmlPath"]);
                    XElement xelement = XElement.Load(filePath);

                    //Get fee based on account number
                    string consumerBasedFee = xelement.Elements("consumers").Elements("consumer").
                        Where(e => e.Element("accountNumber") != null && e.Element("accountNumber").Value.Equals(accountNumber))
                        .Select(e => e.Element("fee").Value).FirstOrDefault();

                    //Get fee based on dealer account number
                    string dealerBasedFee = xelement.Elements("Dealers").Elements("Dealer").
                        Where(e => e.Element("dealerAccountNumber") != null && e.Element("dealerAccountNumber").Value.Equals(dealerAccountNumber))
                        .Select(e => e.Element("fee").Value).FirstOrDefault();

                    //Get fee based on state of account holder
                    string stateBasedFee = xelement.Elements("states").Elements("state").
                        Where(e => e.Element("name") != null && e.Element("name").Value.Equals(state))
                        .Select(e => e.Element("fee").Value).FirstOrDefault();

                    //Common Fee
                    string commonFee = xelement.Elements("common").Select(e => e.Element("fee").Value).FirstOrDefault();

                    //Get fee based on payment method either ACH or Card
                    string paymentMethodFee = string.Empty;
                    if (paymentMethod.Equals("ACH", StringComparison.OrdinalIgnoreCase))
                    {
                        paymentMethodFee = xelement.Elements("achPayments").Elements("achPayment")
                            .Where(e => e.Element("type") != null && e.Element("type").Value.Equals(paymentType))
                            .Select(e => e.Element("fee").Value).FirstOrDefault();
                    }
                    else if (paymentMethod.Equals("Card", StringComparison.OrdinalIgnoreCase))
                    {
                        paymentMethodFee = xelement.Elements("cardPayments").Elements("cardPayment")
                             .Where(e => e.Element("type") != null && e.Element("type").Value.Equals(paymentType))
                            .Select(e => e.Element("fee").Value).FirstOrDefault();
                    }

                    if (!string.IsNullOrEmpty(consumerBasedFee))
                    {
                        fee = consumerBasedFee;
                    }
                    else if (!string.IsNullOrEmpty(dealerBasedFee))
                    {
                        fee = dealerBasedFee;
                    }
                    else if (!string.IsNullOrEmpty(stateBasedFee))
                    {
                        fee = stateBasedFee;
                    }
                    else if (!string.IsNullOrEmpty(paymentMethodFee))
                    {
                        fee = paymentMethodFee;
                    }
                    else
                    {
                        fee = commonFee;
                    }
                }
                else
                {
                    fee = "6.00";
                }
                return fee;
            }
            catch
            {
                throw;
            }
        }

        public string ValidateTransaction(string accountNumber, decimal amount, string futurePaymentDate, bool IsTotalAmountDue)
        {
            try
            {
                decimal totalTransaction = 0;
                decimal minAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["minimumAmount"]);
                decimal maxAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["maximumAmount"]);
                int maxTransactionAllowedInADay = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxTransactionAllowedInADay"]);
                if (IsTotalAmountDue && (minAmount > amount || maxAmount < amount))
                {
                    return ResourceFile.Common.LoginAcceptedPaymentsFaild;
                }
                if (!IsTotalAmountDue)
                {
                    if (amount < minAmount)
                    {
                        return "Minimum amount allowed: $" + minAmount;
                    }
                    else if (amount > maxAmount)
                    {
                        return "Maximum amount allowed: $" + maxAmount;
                    }
                }

                if (string.IsNullOrEmpty(futurePaymentDate))
                {
                    var AcctNo = new SqlParameter
                    {
                        ParameterName = "AcctNo",
                        Value = accountNumber.OrDbNull()
                    };
                    object[] parameters = new object[] { AcctNo };
                    var ValidateTransaction = dataCollection.ValidateTransaction<ValidateTransactionEntity>(parameters);
                    if (ValidateTransaction.Count > 0)
                    {
                        foreach (ValidateTransactionEntity transaction in ValidateTransaction)
                        {
                            totalTransaction = Convert.ToInt32(transaction.TotTran);
                        }
                        if (totalTransaction < maxTransactionAllowedInADay)
                        {
                            return "true";
                        }
                        else
                            return "Number of transactions per day exceeded(2 max).";
                    }
                    else return "true";
                }
                else return "true";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
