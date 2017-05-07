using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public interface IDataCollection
    {
        List<T> Login<T>(params object[] parameters);
        List<T> GetAccountDetails<T>(params object[] parameters);
        List<T> VerifyAccount<T>(params object[] parameters);
        string SaveAchPayment(params object[] parameters);
        string CardSavePayment(params object[] parameters);
        void SaveCardTransactionLog(params object[] parameters);
        List<T> VerifyRoutingNumber<T>(params object[] parameters);
        List<T> ValidateTransaction<T>(params object[] parameters);
        List<T> PaymentHistory<T>(params object[] parameters);
        List<T> LastPaymen<T>(params object[] parameters);
        List<T> LoanPayment<T>(params object[] parameters);
        List<T> GetUserProfile<T>(params object[] parameters);
        void UpdateUserProfile(params object[] parameters);
        string UpdateSecurityQuestions(params object[] parameters);
        string SaveSecurityToken(params object[] parameters);
        List<T> GetSecurityToken<T>();
        string SaveTextLog(params object[] parameters);
        string OptInSubscriber(params object[] parameters);
        string OptOutSubscriber(params object[] parameters);
        List<T> GetSubscriberByAccountNumber<T>(params object[] parameters);
        List<T> GetCardInfo<T>(params object[] parameters);
        List<T> GetSearchPayment<T>(params object[] parameters);
        List<T> DuplicatePayment<T>(params object[] parameters);
        string UpdateAdditionalNotifications(params object[] parameters);
        List<T> GetAdditionalNotificationsByAccountNumber<T>(params object[] parameters);
        List<T> GetAchDetail<T>(params object[] parameters);
        List<T> CardDetails<T>(params object[] parameters);
        string SaveAchAutoPaySetup(params object[] parameters);
        List<T> DeleteAutoPaySetup<T>(params object[] parameters);
        void DeleteCard(params object[] parameters);
        List<T> GetAutoPayDetail<T>(params object[] parameters);
        List<T> GetInsuranceInformationDetail<T>(params object[] parameters);
        string SaveInsuranceInformation(params object[] parameters);
        string UpdateAchAutoPaySetup(params object[] parameters);
        void UpdateAutoPayCardSchedule(params object[] parameters);
        /// <summary>
        /// Method to delete saved ach details
        /// </summary>
        /// <param name="parameters">parameters array</param>
        /// <returns></returns>
        string DeleteSavedAch(params object[] parameters);
        void SaveCardBillingAddress(params object[] parameters);
        List<T> GetBillingAddress<T>(params object[] parameters);
        void InsertAutoPayCardSchedule(params object[] parameters);
        List<T> GetInsuranceCompanyList<T>();
        List<T> GetAlerts<T>(params object[] parameters);
        List<T> CheckCardMaxLimit<T>(params object[] parameters);
        string SaveCallbackUrlInfo(params object[] parameters);
        string SaveStatusUrlInfo(params object[] parameters);
        void CancelSchedulePayment(params object[] parameters);
        List<T> GetFuturePayAccountInfo<T>(params object[] parameters);
        List<T> GetAutoPayPaymentMethod<T>(params object[] parameters);
        string UpdateAutoPayWebConfirmationNumber(params object[] parameters);

        //Pay By Text
        string SavePayByTextACHSetup(params object[] parameters);
        string SavePayByTextCardSetup(params object[] parameters);
        string DeletePayByTextSetup(params object[] parameters);
        List<T> GetPayByTextSetupDetail<T>(params object[] parameters);
        List<T> GetAutoPayDuplicatePayment<T>(params object[] parameters);
        // Save text Message log for Send Text Message
        string SaveOutboundTextMessage(params object[] parameters);
        List<T> CheckNSFStatus<T>(params object[] parameters);
        string UpdatePayByTextACHSetup(params object[] parameters);
        string UpdatePayByTextCardSetup(params object[] parameters);
        List<T> GetScheduledTransactionsForAchPayment<T>();
        List<T> GetScheduledTransactionsForCardPayment<T>();
        void UpdateScheduleTransactionStatus(params object[] parameters);
        List<T> GetActivePayByText<T>();
        List<T> GetPayByTextMessageLog<T>(params object[] parameters);
        string SavePayByTextMessageLog(params object[] parameters);
        void UpdateMAFAccountConsecutiveNSFStatus();
    }
}
