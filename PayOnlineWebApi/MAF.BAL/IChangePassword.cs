using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IChangePassword
    {
        ChangePasswordEntity verify(string accountNumber ,string currentPassword);
        string ResetPassword(ChangePasswordEntity changegePassword);
    }
}
