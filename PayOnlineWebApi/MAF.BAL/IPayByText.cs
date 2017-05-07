using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System.Collections.Generic;

namespace MAF.BAL
{
    public interface IPayByText
    {
        string SavePayByTextWithACH(PaymentConfirmationEntity paymentConfirmation);
        string SavePayByTextWithCard(CardInfoEntity cardInfo, string createdBy);
        string DeletePayByTextSetup(string accountNumber);
        PayByTextEntity GetPayByTextSetup(string accountNumber);
        List<PayByTextEntity> GetActivePayByTextSetup();
        string SavePayByTextMessageLog(PayByTextMessageLogEntity objEntity);
        List<PayByTextMessageLogEntity> GetPayByTextMessageLog(string accountNumber, string textNumber);
    }
}
