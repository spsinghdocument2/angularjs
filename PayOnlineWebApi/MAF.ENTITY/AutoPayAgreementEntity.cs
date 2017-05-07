using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class AutoPayAgreementEntity
    {
        public string Fee { get; set; }
        public string PaymentMethod { get; set; }
        public string AccountNumber { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
