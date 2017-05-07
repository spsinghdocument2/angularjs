using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IPayOnline
    {
        PayonlineEntity GetAccountInformation(string accountNumber);
        List<PaymentHistoryEntity> GetPaymentHistory(string accountNumber);
        PaymentHistoryEntity LastPaymentHistory(string accountNumber);
        List<PaymentHistoryEntity> SearchPaymentHistory(SearchPaymentEntity searchPayment);
        List<SearchPaymentDetailsEntity> SearchPaymentDetails(string AccountNUmber, SearchPaymentEntity searchPayment);
        void CancelSchedulePayment(int id);
        string CheckNSFStatus(string accountNumber);
    }
}
