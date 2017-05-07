using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class Register : IRegister
    {
        private IRepository<tblOnlineUser> _repository = null;
        string accountNumber = string.Empty;
        public Register()
        {
            this._repository = new Repository<tblOnlineUser>();

        }

        #region  registration
        public string userRegister(RegisterEntity registerEntity)
        {
            string message = string.Empty;
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = registerEntity.AccountNumber.OrDbNull()
                  
                };
                var FullName = new SqlParameter
                {
                    ParameterName = "FullName",
                    Value = registerEntity.FullName.OrDbNull()
                };
                var Password = new SqlParameter
                {
                    ParameterName = "Password",
                    Value = registerEntity.Password.OrDbNull()
                };
                var Email = new SqlParameter
                {
                    ParameterName = "Email",
                    Value = registerEntity.Email.OrDbNull()
                };
                var SecurityID = new SqlParameter
                {
                    ParameterName = "SecurityID",
                    Value = registerEntity.SecurityID
                };
                var Answer = new SqlParameter
                {
                    ParameterName = "Answer",
                    Value = registerEntity.Answer.OrDbNull()
                };

             
                var SecurityID2 = new SqlParameter
                {
                    ParameterName = "SecurityID2",
                    Value = registerEntity.SecurityID2
                };
                var Answer2 = new SqlParameter
                {
                    ParameterName = "Answer2",
                    Value =  registerEntity.Answer2.OrDbNull()
                };

                var SecurityID3 = new SqlParameter
                {
                    ParameterName = "SecurityID3",
                    Value = registerEntity.SecurityID3
                };
                var Answer3 = new SqlParameter
                {
                    ParameterName = "Answer3",
                    Value = registerEntity.Answer3.OrDbNull()
                };


                var BitReset = new SqlParameter
                {
                    ParameterName = "BitReset",
                    Value = "0"
                };
                //-----------------------------


                object[] parameters = new object[] { AcctNo, FullName, Password, Email, SecurityID, Answer, SecurityID2, Answer2, SecurityID3, Answer3, BitReset };
                var registrationUser = _repository.SpExecuteSql("exec sp_PayOnlineRegister @AcctNo, @FullName,@Password,@Email,@SecurityID,@Answer,@SecurityID2,@Answer2,@SecurityID3,@Answer3,@BitReset", parameters);

                if (registrationUser == "complete")
               {
                SendMail.Instance.Register(registerEntity.FullName, registerEntity.AccountNumber, registerEntity.Email, registerEntity.Password);
                message = MAF.BAL.ResourceFile.Common.RegistrationSuccessful; // "Registration Successful";
              }
             else
             {
                 message = MAF.BAL.ResourceFile.Common.registeredFaild;// "Make sure your account hasn't been registered already.";
             }
            
            }
            catch (Exception ex)
            {
                message = MAF.BAL.ResourceFile.Common.registeredFaild; ;
            
            }

            return message;
        }

        #endregion

    }
}
