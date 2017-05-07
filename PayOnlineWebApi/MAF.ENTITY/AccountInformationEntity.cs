using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class AccountInformationEntity
    {
        public string CheckingAccountNumber { get; set; }
        public string Amount { get; set; }
        public string AccountNumber { get; set; }
        public string Routing { get; set; }
        public string FuturePaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsFuturePayWithAgreement { get; set; }
        public bool IsTotalAmountDue { get; set; }  

    }
}
