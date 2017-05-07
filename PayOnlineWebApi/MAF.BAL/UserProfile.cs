
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MAF.DAL;
using MAF.ENTITY;
using System.Data.SqlClient;

namespace MAF.BAL
{
    public class UserProfile: IUserProfile
    {
          private IDataCollection idata = null;
          public UserProfile()
          {
              this.idata = new DataCollection();
          }

          #region Get User Information 
           public UserProfileEntity GetUserProfile(string accountNo)
          {
              try
              {
                  //var userInfoEnity = new UserProfileEntity();
                  var accountNumber = new SqlParameter
                  {
                      ParameterName = "accountNo",
                      Value = accountNo.OrDbNull()
                  };

                  object[] parameters = new object[] { accountNumber };
                  var userInfo = idata.GetUserProfile<UserProfileEntity>(parameters).FirstOrDefault();
                  return userInfo;
              }
              catch (Exception ex)
              {
                  throw;

              }
              
          }
          #endregion



           #region Update User Information
           public void UpdateUserProfile(string email, string cellNumber, string textNumber, string textNumber2, string address1, string accountNumber, string profilePicture, string address2 , string city , string state , string zip)
           {
               try
               {
                   //var userInfoEnity = new UserProfileEntity();
                   var Email = new SqlParameter
                   {
                       ParameterName = "Email",
                       Value = email.OrDbNull()
                   };

                   var CellNumber = new SqlParameter
                   {
                       ParameterName = "CellNumber",
                       Value = cellNumber.OrDbNull()
                   };

                   var TextNumber = new SqlParameter
                   {
                       ParameterName = "TextNumber",
                       Value = textNumber.OrDbNull()
                   };
                   var TextNumber2 = new SqlParameter
                   {
                       ParameterName = "TextNumber2",
                       Value = textNumber2.OrDbNull()
                   };

                   var Address1 = new SqlParameter
                   {
                       ParameterName = "Address1",
                       Value = address1.OrDbNull()
                   };

                   var AccountNumber = new SqlParameter
                   {
                       ParameterName = "AccountNumber",
                       Value = accountNumber.OrDbNull()
                   };
                   var ProfilePicture = new SqlParameter
                   {
                       ParameterName = "ProfilePicture",
                       Value = profilePicture.OrDbNull()
                   };
                   var Address2 = new SqlParameter
                   {
                       ParameterName = "Address2",
                       Value = address2.OrDbNull()
                   };
                   var City = new SqlParameter
                   {
                       ParameterName = "City",
                       Value = city.OrDbNull()
                   };
                   var State = new SqlParameter
                   {
                       ParameterName = "State",
                       Value = state.OrDbNull()
                   };
                   var Zip = new SqlParameter
                   {
                       ParameterName = "Zip",
                       Value = zip.OrDbNull()
                   };
                   object[] parameters = new object[] { Email, CellNumber, TextNumber, TextNumber2, Address1, AccountNumber, ProfilePicture , Address2, City , State , Zip };
                   idata.UpdateUserProfile(parameters);
               }
               catch (Exception ex)
               {
                   throw;

               }
           }
           #endregion

        #region UPDATE  SECURITY QUESTIONS
           // UPDATE  SECURITY QUESTIONS 
           public string UpdateSecurityQuestion(ForgotConformationEntity model)
           {
               try
               {
                   string message = string.Empty;

                   // Add parameters in SECURITY QUESTIONS  Update
                   var AcctNo = new SqlParameter
                   {
                       ParameterName = "AcctNo",
                       Value = model.AccountNumber.OrDbNull()
                   };
                   var SecurityID = new SqlParameter
                   {
                       ParameterName = "SecurityID",
                       Value = model.SecurityID.OrDbNull()
                   };
                   var Answer = new SqlParameter
                   {
                       ParameterName = "Answer",
                       Value = model.Answer.OrDbNull()
                   };
                   var SecurityID2 = new SqlParameter
                   {
                       ParameterName = "SecurityID2",
                       Value = model.SecurityID2.OrDbNull()

                   };
                   var Answer2 = new SqlParameter
                   {
                       ParameterName = "Answer2",
                       Value =  model.Answer2.OrDbNull()
                   };
                   var SecurityID3 = new SqlParameter
                   {
                       ParameterName = "SecurityID3",
                       Value = model.SecurityID3.OrDbNull() 

                   };
                   var Answer3 = new SqlParameter
                   {
                       ParameterName = "Answer3",
                       Value = model.Answer3.OrDbNull() 
                   };

                   object[] parameters = new object[] { AcctNo, SecurityID, Answer, SecurityID2, Answer2, SecurityID3, Answer3 };
                   message = idata.UpdateSecurityQuestions(parameters);

                   return message;
               }
               catch (Exception ex)
               {
                   throw;

               }
           }

        #endregion

    }
}

