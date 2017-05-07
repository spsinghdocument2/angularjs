namespace MAF.ENTITY
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PayByTextEntity
    {
        public int PayByTextId { get; set; }
        public string SubscriptionId { get; set; }
        public string AcctNo { get; set; }
        public string TextNumber { get; set; }
        public string BankABA { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string ConfirmationNumber { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DueDate { get; set; }
    }
}
