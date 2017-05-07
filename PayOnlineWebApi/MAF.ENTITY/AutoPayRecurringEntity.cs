using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public  class AutoPayRecurringEntity
    {

        public string ScheduleId { get; set; }
        public string TokenId { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }
        public string ScheduleMethod { get; set; }
        public bool IsSchedule { get; set; }
        public bool IsReccuring { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ConfirmationID { get; set; }
        public string AcctNo { get; set; }
        public Nullable<System.DateTime> TranDate { get; set; }
        public Nullable<decimal> TranPayment { get; set; }
        public Nullable<decimal> TranFee { get; set; }
        public string BankABA { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string Status { get; set; }
        public bool SaveAccountFuture { get; set; }
        public string BankAccountName { get; set; }
        public string paymentFrequency { get; set; }
        public Nullable<System.DateTime> ScheduledDate { get; set; }
        public string Messages { get; set; }
        public string confirmationNumber { get; set; }
    }
}
