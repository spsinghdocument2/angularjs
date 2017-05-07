using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public class DataCollection : IDataCollection
    {
          private IRepository<tblOnlineUser> _repository = null;
        private ITextNotificationRepository<tblOnlineUser> textNotificationRepository = null;
          string message = string.Empty;
          public DataCollection()
          {
              this._repository = new Repository<tblOnlineUser>();
            this.textNotificationRepository = new TextNotificationRepository<tblOnlineUser>();
          }

          // This stored procedure Verify account information
          public List<T> Login<T>(params object[] parameters)
          {
              try
              {
                  return _repository.SQLQuery<T>("exec sp_PayOnlineLogin2 @AcctNo,@Username,@Password ", parameters);
              }
              catch
              {
                  throw;
              }
          }

          // This stored procedure Verify account information
        public List<T> VerifyAccount<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec sp_PayOnlineVerify @AcctNo,@LastName", parameters);
             }
             catch
             {
                 throw;
             }
         }
        
        /// <summary>
        /// Get account details based on account number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetAccountDetails<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetPayOnlineAccountDetails @AcctNo", parameters);
            }
            catch
            {
                throw;

            }
        }

        // This stored procedure get search payment details
        public  List<T> GetSearchPayment<T>(params object[] parameters)
        {
            try
            {
                
                return _repository.SQLQuery<T>("exec USP_GetPaymentSchedule @Acctno", parameters);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        

             // This stored procedure Cancel Schedule Payment
        public void CancelSchedulePayment(params object[] parameters)
        {
            try
            {

                _repository.SpExecuteSql("exec USP_CancelSchedulePayment @Id", parameters);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
         // This stored procedure get bank routing number
         public List<T> VerifyRoutingNumber<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec sp_PayOnlineABA @ABA", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }
         }

         // This stored procedure get user account details
         public List<T> GetUserProfile<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec SP_GetUserProfile @accountNo", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }

         }

         // This stored procedure get card account details
         public List<T> GetCardInfo<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec sp_GetCardInfo @accountNo", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }

         }

         // This stored procedure check card max limit in a day.
         public List<T> CheckCardMaxLimit<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec USP_CheckCardMaxLimit @AcctNo,@CardNumber ,@CardName ,@CardExpiry ,@TokenId", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }

         }

         // This stored procedure get billing address details
         public List<T> GetBillingAddress<T>(params object[] parameters)
         {
             try
             {
                 return _repository.SQLQuery<T>("exec USP_GetBillingAddress @AcctNo , @TokenId", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }

         }

         // This stored procedure update user account details
         public void UpdateUserProfile(params object[] parameters)
         {
             try
             {
                 string result = _repository.SpExecuteSql("exec SP_UpdateUserProfile @Email ,@CellNumber, @TextNumber , @TextNumber2,  @Address1 , @AccountNumber ,@ProfilePicture , @Address2, @City , @State , @Zip ", parameters);
             }
             catch (Exception ex)
             {
                 throw;

             }
         }                                                      
                                                                
        // This stored procedure SECURITY QUESTIONS  Update details
        public string UpdateSecurityQuestions(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_updateSecurityQuestions @AcctNo ,@SecurityID, @Answer , @SecurityID2 , @Answer2, @SecurityID3,@Answer3", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

         // This stored procedure save payment                  
        public string SaveAchPayment(params object[] parameters)   
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_PayOnlinePayment @Confirmation, @AcctNo, @TranPayment, @TranFee, @BankABA, @BankAcctNo, @BankName, @BankHolder, @BankAcctType, @UpdatedPhone, @UpdatedPhoneDate ,@SaveAccountFuture,@BankAccountName,@FuturePaymentDate,@ScheduleMethod,@Email", parameters);
            }
            catch 
            {
                throw;

            }
        }


        // This stored procedure save payment                  
        public string CardSavePayment(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec USP_InsertCardSubscription @ConfirmationId, @AcctNo,  @TokenId, @CardName, @CardType, @CardNumber, @CardExpiry,  @FuturePaymentDate,@Fee,@Status, @Amount ", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        // This stored procedure save Card Transaction Log                  
        public void SaveCardTransactionLog(params object[] parameters)
        {
            try
            {
                _repository.SpExecuteSql("exec USP_InsertCardTransactionLog @AcctNo,@ConfirmationId,@TokenId,@RequestId,@CardName,@CardNumber,@CardExpiry,@CardType,@ReasonCode,@Description", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

         // This stored procedure save Card BillingAddress                  
        public void SaveCardBillingAddress(params object[] parameters)
        {
            try
            {
                _repository.SpExecuteSql("exec USP_InsertBillingAddress   @TokenId,@ConfirmationId,@AcctNo,  @FirstName, @LastName, @PrimaryNumber, @EmailID, @Address, @City, @State, @Zip, @Country", parameters);                
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        // This stored procedure InsertAutoPayCardSchedule Card BillingAddress                  
        public void InsertAutoPayCardSchedule(params object[] parameters)
        {
            try
            {
                _repository.SpExecuteSql("exec USP_InsertAutoPayCardSubscription   @ConfirmationId,@Acctno,  @TokenId, @CardName, @CardType, @CardNumber, @CardExpiry, @Amount,@paymentFrequency,@TranFee", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        // This stored procedure update AutoPayCardSchedule Card                  
        public void UpdateAutoPayCardSchedule(params object[] parameters)
        {
            try
            {
                _repository.SpExecuteSql("exec USP_UpdateAutoPayCardSubscription  @ConfirmationId,@Acctno, @TokenId, @CardName, @CardType,@CardNumber, @CardExpiry, @Amount,@paymentFrequency,@TranFee,@ScheduleID", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        // Get Last payment Amount
        public void DeleteCard(params object[] parameters)
        {
            try
            {
                _repository.SpExecuteSql("exec USP_DeleteCard @tokenId", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        // Get validation maximum Transaction in payment
        public List<T> ValidateTransaction<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetValidateTransaction @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        // Get Payment History information
        public List<T> PaymentHistory<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetPaymentHistory @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        // Get Last Payment Information
        public List<T> LastPaymen<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetLastPaymentHistory @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        // Get Loan Payment and Account information 
        public List<T> LoanPayment<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetMafAccountDetails @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        /// <summary>
        /// Method to save security token in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">Object array of parameters</param>
        /// <returns></returns>
        public string SaveSecurityToken(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTSaveSecurityToken @SecurityToken, @TokenExpireInMinutes", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get security token from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetSecurityToken<T>()
        {
            try
            {
                return textNotificationRepository.SQLQuery<T>("exec SBTGetSecurityToken");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save text log for message opt-in/out
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">Parameters</param>
        /// <returns></returns>
        public string SaveTextLog(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTInsertTextLog @AccountNumber, @TextNumber, @TextLogType, @TextLogEntry, @TextLogUserName", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to opt in
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public string OptInSubscriber(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTOptInSubscriber @AccountNumber, @TextNumber, @OptInIPAddress, @CreatedBy, @IsPaymentReminderNotification, @IsPaymentConfirmationNotification, @IsPayByTextNotification, @IsCommunicationByTextNotification", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Opt Out
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public string OptOutSubscriber(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTOptOutSubscriber @SubscriberID, @OptOutIPAddress, @ModifiedBy", parameters);
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Method to Get Subscriber details
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetSubscriberByAccountNumber<T>(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SQLQuery<T>("exec SBTGetSubscriberByAccount @AccountNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Get Additional Notifications details
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetAdditionalNotificationsByAccountNumber<T>(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SQLQuery<T>("exec SBTGetAdditionalNotifications @AccountNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Update Additional Notifications
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public string UpdateAdditionalNotifications(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTUpdateAdditionalNotification @SubscriberID, @IsPaymentReminderNotification, @IsPaymentConfirmationNotification, @IsPayByTextNotification, @IsCommunicationByTextNotification", parameters);
            }
            catch
            {
                throw;
            }
        }

        // This stored procedure get Duplicate Payment
        public List<T> DuplicatePayment<T>(params object[] parameters)
        {
         
            try
            {
                return _repository.SQLQuery<T>("exec sp_CheckDuplicatePayment @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }

        }

        // This stored procedure get Duplicate Payment
        public List<T> GetAutoPayDuplicatePayment<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec sp_AutoPayDuplicatePaymentAlert @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }

        }
    
        // This stored procedure get Card Details
        public List<T> CardDetails<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec USP_GetCardSbscriptionInfo @AcctNo", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }

        }
    
    
        /// <summary>
        /// GET Ach Detail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetAchDetail<T>(params object[] parameters)
        {
          
            try
            {
                return _repository.SQLQuery<T>("exec SP_GetAchDetail @AcctNo", parameters);
            }
            catch
            {
                throw;
            }
        }


        // This stored procedure save payment autopay                 
        public string SaveAchAutoPaySetup(params object[] parameters)
        {
            
            try
            {
                return _repository.SpExecuteSql("exec sp_ACHAutoPay_Insert @Confirmation, @AcctNo, @TranPayment, @TranFee, @BankABA, @BankAcctNo, @BankName, @BankHolder, @BankAcctType, @UpdatedPhone, @UpdatedPhoneDate ,@SaveAccountFuture,@BankAccountName,@ScheduleMethod,@Email,@Filter,@paymentFrequency,@Message OUT", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        // This stored procedure update payment autopay                 
        public string UpdateAchAutoPaySetup(params object[] parameters)
        {

            try
            {
                return _repository.SpExecuteSql("exec sp_UpdateAutoPayAch @Confirmation, @AcctNo, @TranPayment,@TranFee,@BankABA,@BankAcctNo,@BankName,@BankHolder,@BankAcctType,@BankAccountName,@ScheduleMethod,@Email,@paymentFrequency,@ScheduleID,@SaveAccountFuture,@UpdatedPhone", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        // This stored procedure delete payment                  
        public List<T> DeleteAutoPaySetup<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec sp_DeleteAutoPayRecurring  @AcctNo,@Message OUT", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        /// <summary>
        /// Get Auto Pay Recurring Detail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetAutoPayDetail<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec sp_GetAutoPayRecurring @AcctNo", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to delete saved ach details
        /// </summary>
        /// <param name="parameters">parameters array</param>
        /// <returns></returns>
        public string DeleteSavedAch(params object[] parameters)
        {
          
            try
            {
                return _repository.SpExecuteSql("exec sp_DeleteSavedAchDetail @AcctNo, @ID", parameters);
            }
            catch
            {
                throw;
            }
        }

        
              /// <summary>
        /// Get Insurance company Name List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">null</param>
        /// <returns></returns>
        public List<T> GetInsuranceCompanyList<T>()
        {
          
            try
            {
                return _repository.SQLQuery<T>("exec USP_GetInsuranceCompanyList");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Insurance Information Detail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T>  GetInsuranceInformationDetail<T>(params object[] parameters)
        {
          
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetInsuranceInformation @AcctNo", parameters);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// save Insurance Information 
        /// </summary>
        /// <typeparam ></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>                
        public string SaveInsuranceInformation(params object[] parameters)
        {

            try
            {
                return _repository.SpExecuteSql("exec sp_InsertUpdateInsuranceInformation @AcctNo,@InsPolicyNo,@InsEffDate,@InsExpDate,@InsuranceCompanyName,@InsuranceCompanyAddress, @InsuranceCompanyCity,@InsuranceCompanyState,@InsuranceCompanyZip, @InsuranceAgencyName, @InsuranceAgencyAddress,@InsuranceAgencyCity,@InsuranceAgencyState,@InsuranceAgencyZip, @InsuredName,@InsuredAddress,@InsuredCity,@InsuredState ,@InsuredZip, @InsuranceCardPath,@Filter", parameters);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        /// <summary>
        /// Method to get alerts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetAlerts<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec sp_GetAlerts @AcctNo", parameters);
            }
            catch
            {
                throw;

            }

        }


        /// <summary>
        /// Method to Save callback url info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public string SaveCallbackUrlInfo(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTSaveCallBackUrlInfo @Title, @Code, @ShortCode, @Message, @Phone, @Carrier, @Keyword, @Group, @Note, @UniqueId, @Default_Keyword ", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Save satus url info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public string SaveStatusUrlInfo(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTSaveStatusUrlInfo @Code, @Message, @SendTo, @OrgCode,  @Status, @Ticket, @Note, @UniqueId, @MessageCategory, @MessageSubCategory ", parameters);
            }
            catch
            {
                throw;
            }
        }


        #region FuturePay
        // This stored procedure get future pay account details
        public List<T> GetFuturePayAccountInfo<T>(params object[] parameters)
        {
            try
            {

                return _repository.SQLQuery<T>("exec SP_GetFuturePayAccountInfo @Acctno", parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Method to get autopay payment method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns>list of results</returns>
        public List<T> GetAutoPayPaymentMethod<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetAutoPaySetupPaymentMethod @accountNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update auto pay web confirmation number from autopay agreement
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string UpdateAutoPayWebConfirmationNumber(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_UpdateAutoPayConfirmationNumber @accountNumber, @confirmationNumber ", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Pay By Text setup Detail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetPayByTextSetupDetail<T>(params object[] parameters)
        {

            try
            {
                return _repository.SQLQuery<T>("exec sp_GetPayByTextSetup @AcctNo", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to save pay by text setup for ACH payment
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SavePayByTextACHSetup(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_InsertPayByTextACHSetup @Confirmation, @AcctNo, @BankABA, @BankAcctNo, @BankName, @BankHolder, @BankAcctType, @CreatedBy, @Phone, @Email, @TextNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to save pay by text setup for Card payment
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SavePayByTextCardSetup(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_InsertPayByTextCardSetup @Confirmation, @AcctNo, @TokenId, @CardName, @CardType, @CardNumber, @CardExpiry, @CreatedBy, @TextNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update pay by text setup for ACH payment
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string UpdatePayByTextACHSetup(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_UpdatePayByTextACHSetup @Confirmation, @AcctNo, @BankABA, @BankAcctNo, @BankName, @BankHolder, @BankAcctType, @CreatedBy, @Phone, @Email, @TextNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update pay by text setup for Card payment
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string UpdatePayByTextCardSetup(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_UpdatePayByTextCardSetup @Confirmation, @AcctNo, @TokenId, @CardName, @CardType, @CardNumber, @CardExpiry, @CreatedBy, @TextNumber", parameters);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Method contains stored procedure and parameters with query to delete pay by text setup
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DeletePayByTextSetup(params object[] parameters)
        {

            try
            {
                return _repository.SpExecuteSql("exec sp_DeletePayByTextSetup  @AcctNo", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save text Message log for Send Text Message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">Parameters</param>
        /// <returns></returns>
        public string SaveOutboundTextMessage(params object[] parameters)
        {
            try
            {
                return textNotificationRepository.SpExecuteSql("exec SBTInsertSendTextMessage @AccountNumber,@TextNumber,@Textmessage", parameters);
            }
            catch
            {
                throw;
            }
        }


        // This stored procedure Check NSF Status
        public List<T> CheckNSFStatus<T>(params object[] parameters)
        {
            try
            {

                return _repository.SQLQuery<T>("exec SP_CheckNSFStatus @AcctNo", parameters);
            }
            catch 
            {
                throw;
            }
        }


        // This stored procedure Get Scheduled Transactions For Ach Payment
        public List<T> GetScheduledTransactionsForAchPayment<T>()
        {
            try
            {

                return _repository.SQLQuery<T>("exec sp_GetScheduledTransactionsForAchPayment");
            }
            catch
            {
                throw;
            }
        }

        // This stored procedure Get Scheduled Transactions For Card Payment
        public List<T> GetScheduledTransactionsForCardPayment<T>()
        {
            try
            {

                return _repository.SQLQuery<T>("exec sp_GetScheduledTransactionsForCardPayment");
            }
            catch
            {
                throw;
            }
        }


        // This stored procedure Update Schedule Transaction Status
        public void UpdateScheduleTransactionStatus(params object[] parameters)
        {
            try
            {

                _repository.SpExecuteSql("exec sp_UpdateScheduleTransactionStatus @ScheduleDetailsId,@Status", parameters);
            }
            catch
            {
                throw;
            }
        }

        // This stored procedure Update MAF Account Consecutive NSF Status
        public void UpdateMAFAccountConsecutiveNSFStatus()
        {
            try
            {

                _repository.SpExecuteSql("exec sp_UpdateMAFAccountConsecutiveNSFStatus");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Get Active Pay By Text
        /// </summary>
        /// <returns></returns>
        public List<T> GetActivePayByText<T>()
        {
            try
            {
                return textNotificationRepository.SQLQuery<T>("exec sp_GetActivePayByTextSetup");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Pay By Text Message Log Detail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">object array of parameters</param>
        /// <returns></returns>
        public List<T> GetPayByTextMessageLog<T>(params object[] parameters)
        {
            try
            {
                return _repository.SQLQuery<T>("exec sp_GetPayByTextMessageLog @AcctNo, @TextNumber", parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to save pay by text message log
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SavePayByTextMessageLog(params object[] parameters)
        {
            try
            {
                return _repository.SpExecuteSql("exec sp_InsertPayByTextMessageLog @AcctNo, @TextNumber, @Message, @Type, @Keyword", parameters);
            }
            catch
            {
                throw;
            }
        }
    }
}
