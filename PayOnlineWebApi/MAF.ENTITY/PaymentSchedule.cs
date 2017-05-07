using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class PaymentSchedule
    {        
        public string Date { get; set; }
        public string Payment { get; set; }
        public string Fee { get; set; }
        public string BankABA { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string Status { get; set; }
    }
}
