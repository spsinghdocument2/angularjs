using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IAutoPayAgreement
    {
        string GetAutoPayPaymentMethod(string accountNumber);
        string UpdateAutoPayConfirmationNumber(string accountNumber, string confirmationNumber);
    }
}
