﻿using MAF.DAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IFuturePay
    {
        FuturePayEntity GetAccountInfo(int accountNumber);
    }
}
