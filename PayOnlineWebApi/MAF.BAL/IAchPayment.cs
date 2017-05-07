namespace MAF.BAL
{
    using MAF.ENTITY;
    using System.Collections.Generic;

    public interface IAchPayment
    {
        List<AchDetailEntity> GetSavedAchDetail(string accountNumber);
        string DeleteSavedAchDetail(long id, int accountNumber);
        string SavePayment(PaymentConfirmationEntity paymentConfirmation);
        RoutingNumberEntity ValidatePayment(AccountInformationEntity accountInformation);
        RoutingNumberEntity ValidateRoutingNumber(string routingNumber);
        List<AchDetailEntity> GetScheduledTransactionsForAchPayment();
    }
}
