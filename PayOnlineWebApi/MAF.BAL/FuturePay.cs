using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MAF.ENTITY.CyberSource;

namespace MAF.BAL
{
    public class FuturePay : IFuturePay
    {
        private IDataCollection idata = null;
        public FuturePay()
        {
            this.idata = new DataCollection();
        }
        #region Get Account Information
        public FuturePayEntity GetAccountInfo(int accountNumber)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };
                object[] parameters = new object[] { AcctNo };
                return idata.GetFuturePayAccountInfo<FuturePayEntity>(parameters).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
