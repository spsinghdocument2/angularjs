using System;
using MAF.ENTITY;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public interface IUserProfile
    {
        UserProfileEntity GetUserProfile(string accountNumber);
        void UpdateUserProfile(string email, string cellNumber, string textNumber, string textNumber2, string address1, string accountNumber, string profilePicture ,string address2 , string city , string state , string zip);
        string UpdateSecurityQuestion(ForgotConformationEntity model);

    }
}
