using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class LoanPaymentEntity
    {
        public string AcctCurrentBal { get; set; }
        public string AcctDaysPastDue { get; set; }
        public string AcctPastDueAmt { get; set; }
        public string ConfirmationID { get; set; }
        public Nullable<System.DateTime> TranDate { get; set; }
        public string TranPayment { get; set; }
        public string TranFee { get; set; }
        public string BankABA { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string Status { get; set; }
        public string AcctFreq { get; set; }
        public Nullable<System.DateTime> AcctNextDueDate { get; set; }
        public string ActionLogSubType { get; set; }
        public string LastTransactionDate { get; set; }
        public string NextDueDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}
