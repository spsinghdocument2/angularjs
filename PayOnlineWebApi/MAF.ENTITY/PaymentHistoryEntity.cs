using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
     public class PaymentHistoryEntity
    {
        // select ID, ConfirmationID,AcctNo,TranDate,TranPayment,TranFee,BankABA,BankAcctNo,BankName,BankHolder,BankAcctType,Status from tblOnlinePayments
        public Int32 ID { get; set; }
        public string ConfirmationID { get; set; }
        public string TranDate { get; set; }
        public string TranPayment { get; set; }
        public string TranFee { get; set; }
        public string BankABA { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankHolder { get; set; }
        public string BankAcctType { get; set; }
        public string Status { get; set; }

    }
}
