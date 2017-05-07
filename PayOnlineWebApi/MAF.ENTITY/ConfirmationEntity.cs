using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public  class ConfirmationEntity
    {
        //ConfirmationID
        public string Confirmation { get; set; }
        public string AccountNumber { get; set; }
        public string TranPayment { get; set; }
        public string TranFee { get; set; }
        public string BankABA { get; set; }
        public string BankAccounttNumber { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string UpdatedPhone { get; set; }
        public string UpdatedPhoneDate { get; set; }

    }
}
