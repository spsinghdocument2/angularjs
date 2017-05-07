using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IForgotPassword
    {

         ForgotConformationEntity verify(string AccountNumber);

       
        string TemporarypasswordSendMail(ChangePasswordEntity changegePassword);
    }
}
