using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class PaymentConfirmationEntity
    {
        public string AccountNumber { get; set; }
        public string PaymentAmount { get; set; }
        public string FeeAmoun { get; set; }
        public string RountingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string AccountType { get; set; }
        public string UpdatedPhone { get; set; }
        public string UpdatedPhoneDate { get; set; }
        public string RecurringPayment { get; set; }
        public string Email { get; set; }
        public string SaveAccountFuture { get; set; }
        public string BankAccountName { get; set; }
        public string FuturePaymentDate { get; set; }
        public string paymentFrequency { get; set; }
        public string Message { get; set; }
        public string IsActive { get; set; }       
        public string ScheduleID { get; set; }
        public string Status { get; set; }
        public string TextNumber { get; set; }
        public string EditStatus { get; set; }
        public string ConfirmationId { get; set; }
    }
}
