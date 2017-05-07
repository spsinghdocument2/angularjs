using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public  class AchDetailEntity
    {
        public int ID { get; set; }
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
        public string SaveAccountFuture { get; set; }
        public string CreatedBy { get; set; }
        public string Email { get; set; }
        public string PrimaryNumber { get; set; }
        public bool IsActive { get; set; } 
    }
}
