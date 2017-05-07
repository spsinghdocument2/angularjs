using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class Authenticate : IAuthenticate
    {
        private IDataCollection idata = null;
        public Authenticate()
        {
            this.idata = new DataCollection();
        }

        #region registration Verify
        public AuthenticateConfirmationEntity AuthenticateConformationUser(AuthenticateVerifyEntity authenticateVerifyEntity)
        {
            var authenticateConformationEntity = new AuthenticateConfirmationEntity();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = authenticateVerifyEntity.AccountNumber.OrDbNull()
                };
                var LastName = new SqlParameter
                {
                    ParameterName = "LastName",
                    Value = authenticateVerifyEntity.LastName.ToUpper()
                };

                object[] parameters = new object[] { AcctNo, LastName };
                var PayOnlineVerify = idata.VerifyAccount<PayOnlineVerifyResultEntity>(parameters).FirstOrDefault();
                if (PayOnlineVerify != null)
                {
                    authenticateConformationEntity.AccountNumber = PayOnlineVerify.AcctNo;
                    authenticateConformationEntity.AccountHolder = PayOnlineVerify.FullName;
                    authenticateConformationEntity.VehicleYear = PayOnlineVerify.VehYear.ToString();
                    authenticateConformationEntity.Make = PayOnlineVerify.VehModel;
                    authenticateConformationEntity.Model = PayOnlineVerify.VehType;
                }
                else
                {
                    authenticateConformationEntity.Error = MAF.BAL.ResourceFile.Common.authenticateConformationError;// "Account doesn't exist or Last Name doesn't match";
                }
            }
            catch
            {
                throw;
            }
            return authenticateConformationEntity;
        }
        #endregion
    }
}
