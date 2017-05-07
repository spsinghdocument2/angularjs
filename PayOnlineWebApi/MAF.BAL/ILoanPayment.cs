using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
     public interface ILoanPayment
     {
         LoanPaymentEntity GetLoanPayment(string accountNumber);
         List<DuplicatePaymentEntity> GetDuplicatePayment(string accountNumber);
         List<AlertEntity> GetAlerts(string accountNumber);
     }
}
