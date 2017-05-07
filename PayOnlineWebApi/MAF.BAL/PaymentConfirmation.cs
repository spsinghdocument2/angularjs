using MAF.DAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MAF.BAL
{
    public class PaymentConfirmation : IPaymentConfirmation
    {
        string message = string.Empty;
        private IDataCollection idata = null;
        public PaymentConfirmation()
        {
            this.idata = new DataCollection();
        }

        #region Save Card Info 
       
        #endregion
    }
}
