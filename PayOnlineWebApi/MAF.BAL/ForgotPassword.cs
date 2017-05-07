using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class ForgotPassword : IForgotPassword
    {
        private IRepository<tblOnlineUser> _repository = null;
        ForgotConformationEntity forgotConformationEntity = null;
        public ForgotPassword()
        {
            this._repository = new Repository<tblOnlineUser>();
        }

        #region Forgot PASSWORD Verify
        public ForgotConformationEntity verify(string AccountNumber)
        {
         forgotConformationEntity = new ForgotConformationEntity();
         string accountNumber = AccountNumber.Trim();
         
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                  object[] parameters = new object[] { AcctNo };
                  var PayOnlineForgot = _repository.SQLQuery<sp_PayOnlineForgot_Result>("exec sp_PayOnlineForgot @AcctNo", parameters);
                  if (PayOnlineForgot.Count > 0)
                  {
                   forgotConformationEntity.AccountNumber = PayOnlineForgot.Select(a => a.AcctNo).FirstOrDefault();
                   forgotConformationEntity.Answer = PayOnlineForgot.Select(a => a.Answer).FirstOrDefault();
                   forgotConformationEntity.Email = PayOnlineForgot.Select(a => a.Email).FirstOrDefault();
                   forgotConformationEntity.FullName = PayOnlineForgot.Select(a => a.FullName).FirstOrDefault();
                 //  forgotConformationEntity.Password = PayOnlineForgot.Select(a => a.Password).FirstOrDefault();
                   forgotConformationEntity.SecurityID = PayOnlineForgot.Select(a => Convert.ToString( a.SecurityID)).FirstOrDefault();

                   forgotConformationEntity.SecurityID2 = PayOnlineForgot.Select(a => Convert.ToString(a.SecurityID2)).FirstOrDefault();
                   forgotConformationEntity.Answer2 = PayOnlineForgot.Select(a => a.Answer2).FirstOrDefault();
                   forgotConformationEntity.SecurityID3 = PayOnlineForgot.Select(a => Convert.ToString(a.SecurityID3)).FirstOrDefault();
                   forgotConformationEntity.Answer3 = PayOnlineForgot.Select(a => a.Answer3).FirstOrDefault();

                  
                  }

                  else
                      {
                          forgotConformationEntity.Error = MAF.BAL.ResourceFile.Common.ForgotConformationError;// "Account doesn't match our records";
                      }
            }
            catch(Exception ex)
            {
                throw;
            }


            return forgotConformationEntity;

        }
        #endregion

        #region  create temporary password and send mail
        public string TemporarypasswordSendMail(ChangePasswordEntity changegePassword)
        {
            string Temporarypassword = String.Concat("Maf@", DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
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
                    Value = Temporarypassword.OrDbNull()
                };
                var BitReset = new SqlParameter
                {
                    ParameterName = "BitReset",
                    Value = "1"
                };


                object[] parameters = new object[] { NewOne, AcctNo, BitReset };
                var changePasswordUser = _repository.SpExecuteSql("exec sp_PayOnlineChange @NewOne, @AcctNo, @BitReset", parameters);
                if (changePasswordUser == "complete")
                {

                    message = SendMail.Instance.ChangePassword(changegePassword.FullName, changegePassword.AccountNumber, changegePassword.Email, Temporarypassword);

                }
                else
                {
                    message = MAF.BAL.ResourceFile.Common.ChangegePasswoResetFaild;// "Password Change Failed";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }

        #endregion
    }
}
