using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class PayonlineEntity
    {
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string CurrentBalance { get; set; }
        public string AmountPastDue { get; set; }
        public string RegularAmountDue { get; set; }
        public string OtherAmount { get; set; }
        public string TotalAmountDue { get; set; }
        public string AccountHolderPhoneNumber { get; set; }
        public string  DueDate {get;set;}
        public string LastTransactionFee { get; set; }
        public string LastTransactionPayment { get; set; }
        public string LastBankAccountType { get; set; }
        public string TotalPayoffAmmount { get; set; }
        public string Error { get; set; }
        public string RemainingTerm { get; set; }
        public string NSFStatus { get; set; }
        public string TodayDate { get; set; }
        public int? AcctDaysPastDue { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
    }
}
