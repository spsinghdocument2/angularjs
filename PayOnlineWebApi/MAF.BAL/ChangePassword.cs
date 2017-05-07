using MAF.BAL.ResourceFile;
using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
   public   class ChangePassword : IChangePassword
    {
        private IRepository<tblOnlineUser> _repository = null;
        ChangePasswordEntity changegePasswordEntity = null;
        public ChangePassword()
        {
            this._repository = new Repository<tblOnlineUser>();
        }

        #region  change password verify
        public  ChangePasswordEntity verify(string accountNumber ,string currentPassword)
         {
            changegePasswordEntity = new ChangePasswordEntity();
            string AccountNumber = accountNumber.Trim();

            if (accountNumber.Trim() != String.Empty)
            {
                try
                {
                    var AcctNo = new SqlParameter
                    {
                        ParameterName = "AcctNo",
                        Value = AccountNumber.OrDbNull()
                    };
                    object[] parameters = new object[] { AcctNo };
                    var PayOnlineChangePassword = _repository.SQLQuery<sp_PayOnlineForgot_Result>("exec sp_PayOnlineForgot @AcctNo", parameters);
                     if (PayOnlineChangePassword.Count > 0)
                     {
                         changegePasswordEntity.AccountNumber = PayOnlineChangePassword.Select(a => a.AcctNo).First().ToString();
                         changegePasswordEntity.Email = PayOnlineChangePassword.Select(a => a.Email).First().ToString();
                         changegePasswordEntity.FullName = PayOnlineChangePassword.Select(a => a.FullName).First().ToString();
                         changegePasswordEntity.Password = PayOnlineChangePassword.Select(a => a.Password).First().ToString();
                     
                     
                     }

                     else
                     {
                         changegePasswordEntity.Error = MAF.BAL.ResourceFile.Common.ChangegePassworVerifyError;
                       
                     }
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
             return changegePasswordEntity;
         }
        #endregion

        #region password Reset
        public string ResetPassword(ChangePasswordEntity changegePassword)
        {
            string message = string.Empty;
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = changegePassword.AccountNumber.OrDbNull()

                };
                var NewOne = new SqlParameter
                {
                    ParameterName = "NewOne",
                    Value = changegePassword.NewPassword.OrDbNull()
                };
                var BitReset = new SqlParameter
                {
                    ParameterName = "BitReset",
                    Value = changegePassword.BitReset.OrDbNull()
                };

                object[] parameters = new object[] { NewOne, AcctNo, BitReset };
                var changePasswordUser = _repository.SpExecuteSql("exec sp_PayOnlineChange @NewOne, @AcctNo,@BitReset", parameters);
                if (changePasswordUser == "complete")
                {
                 message =  SendMail.Instance.ChangePassword(changegePassword.FullName, changegePassword.AccountNumber, changegePassword.Email, changegePassword.NewPassword);
                 
                }
                else
                {
                    message = MAF.BAL.ResourceFile.Common.ChangegePasswoResetFaild;// "Password Change Failed";
                }

            }
            catch(Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }
        #endregion
    }
}
