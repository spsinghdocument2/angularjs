using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{

    /// <summary>
    /// GET Ach Detail
    /// </summary>
    public interface IAutoPay
    {
        List<AchDetailEntity> GetAchDetail(string accountNumber);
        string SaveAchAutoPaySetup(PaymentConfirmationEntity paymentConfirmation);
        List<AutoPayRecurringEntity> GetAutoPayDetail(string accountNumber);
        string DeleteAutoPay(string AccountNumber);
        void SaveCardBillingAddress(CardInfoEntity cardInfo);
        void InsertAutoPayCardSchedule(CardInfoEntity cardInfo);
        void SaveCardTransactionLog(CardDetailsEntity cardInfo);
        List<DuplicatePaymentEntity> GetAutoPayDuplicatePayment(string accountNumber);
    }
}
