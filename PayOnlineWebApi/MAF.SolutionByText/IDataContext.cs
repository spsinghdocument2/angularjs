using MAF.ENTITY.SolutionByText;
using System.Collections.Generic;
namespace MAF.SolutionByText
{
    public interface IDataContext
    {
        string SaveTextLog(string accountNumber, string textNumber, string logType, string logEntry, string username);
        string OptInSubscriber(TextNotificationEntity textNotificationEntity, string optInIPAddress, string username);
        string OptOutSubscriber(int subscriberId, string optOutIPAddress, string username);
        List<Subscriber> GetSubscriberByAccountNumber(string accountNumber);
        List<Subscriber> GetAdditionalNotifications(string accountNumber);
        string UpdateAdditionalNotifications(int subscriberId, bool isPaymentReminder, bool isPaymentConfirmation, bool isPayByText, bool isCommunicationByText);
        string SaveCallbackUrlInfo(CallbackUrlInfoEntity objCallbackUrlInfo);
        string SaveStatusUrlInfo(StatusUrlInfoEntity objStatusUrlInfo);
        string SaveTextMessage(string accountNumber, string textNumber, string message);
    }
}
