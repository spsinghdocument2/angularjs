using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MAF.ENTITY.CyberSource;


namespace MAF.BAL
{
   public interface ICardInfo
    {
        CardInfoEntity GetCardInfo(string accountNumber);
        //void UpdateUserProfile(string email, string cellNumber, string textNumber, string address, string accountNumber, string profilePicture);
        //string UpdateSecurityQuestion(ForgotConformationEntity model);
    }
}
