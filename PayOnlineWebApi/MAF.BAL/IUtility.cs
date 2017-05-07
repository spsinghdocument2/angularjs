namespace MAF.BAL
{
    public interface IUtility
    {
        /// <summary>
        /// Method to get the fee for transaction
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <param name="paymentMethod">Payment Method (Either ACH or Card)</param>
        /// <param name="paymentType">Payment Type (Either OneTime or Recurring)</param>
        /// <returns>Fee</returns>
        string GetFeeByPaymentMethodAndPaymentType(string accountNumber, string paymentMethod, string paymentType);
    }
}
