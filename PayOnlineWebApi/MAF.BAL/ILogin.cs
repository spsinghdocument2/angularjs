using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public  interface ILogin
    {

        LoginEntity UserLogin(LoginEntity loginEntiys);
    }

   
}
