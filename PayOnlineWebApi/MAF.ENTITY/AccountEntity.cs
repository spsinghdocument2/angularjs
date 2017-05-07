using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class AccountEntity
    {
        public string AcctNo { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> AcctCurrentBal { get; set; }
        public Nullable<decimal> AcctRegPayAmt { get; set; }
        public Nullable<decimal> AcctPastDueAmt { get; set; }
        public Nullable<int> VehYear { get; set; }
        public string VehModel { get; set; }
        public string VehType { get; set; }
        public string State { get; set; }
        public string DlrAcctNo { get; set; }
        public string UpdatedPhone { get; set; }
        public Nullable<System.DateTime> UpdatedPhoneDate { get; set; }
        public Nullable<System.DateTime> AcctNextDueDate { get; set; }
        public string TranFee { get; set; }
        public string TranPayment { get; set; }
        public string BankAcctType { get; set; }
        public Nullable<decimal> AcctPayOff { get; set; }
        public Nullable<int> RemainingTerm { get; set; }
        public Nullable<int> AcctDaysPastDue { get; set; }
        
    }
}
