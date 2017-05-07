namespace MAF.BAL
{
    using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public interface ICardPayment
    {
        List<CardDetailsEntity> CardDetails(string accountNumber);
        string SaveCardInfo(CardInfoEntity cardInfo);
        void SaveCardBillingAddress(CardInfoEntity cardInfo);
        void SaveCardTransactionLog(CardDetailsEntity cardInfo);
        string CheckCardMaxLimit(CardInfoEntity cardInfo);
        void DeleteCard(string tokenIdValue);
        BillingAddressEntity GetBillingAddress(string accountNumber, string tokenId);
        string CheckValidCardTransaction(string accountNumber, string amount, string futurePaymentDate, bool IsTotalAmountDue);
        List<CardInfoEntity> GetScheduledTransactionsForCardPayment();
        void UpdateScheduleTransactionStatus(string scheduleDetailsId, string status);
    }
}
