using MAF.DAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MAF.BAL
{
    public class AutoPay : IAutoPay
    {
        private IDataCollection dataCollection = null;
        public AutoPay()
        {
            this.dataCollection = new DataCollection();
        }

        /// <summary>
        ///Get Ach Detail
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public List<AchDetailEntity> GetAchDetail(string accountNumber)
        {
            var AchDetail = new List<AchDetailEntity>();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };

                object[] parameters = new object[] { AcctNo };

                AchDetail = dataCollection.GetAchDetail<AchDetailEntity>(parameters);
            }
            catch (Exception ex)
            {
                throw;

            }

            return AchDetail;

        }

        /// <summary>
        /// Method to Save Ach Auto Pay
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>send Confirmation</returns>
        public string SaveAchAutoPaySetup(PaymentConfirmationEntity paymentConfirmation)
        {
            string message = string.Empty;

            string confirmation = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));
            try
            {
                var Confirmation = new SqlParameter
                {
                    ParameterName = "Confirmation",
                    Value = confirmation.OrDbNull()
                };
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = paymentConfirmation.AccountNumber.OrDbNull()
                };
                var TranPayment = new SqlParameter
                {
                    ParameterName = "TranPayment",
                    Value = paymentConfirmation.PaymentAmount.OrDbNull()
                };
                var TranFee = new SqlParameter
                {
                    ParameterName = "TranFee",
                    Value = paymentConfirmation.FeeAmoun.OrDbNull()
                };
                var BankABA = new SqlParameter
                {
                    ParameterName = "BankABA",
                    Value = paymentConfirmation.RountingNumber.OrDbNull()
                };
                var BankAcctNo = new SqlParameter
                {
                    ParameterName = "BankAcctNo",
                    Value = paymentConfirmation.BankAccountNumber.OrDbNull()
                };
                var BankName = new SqlParameter
                {
                    ParameterName = "BankName",
                    Value = paymentConfirmation.BankName.OrDbNull()
                };
                var BankHolder = new SqlParameter
                {
                    ParameterName = "BankHolder",
                    Value = paymentConfirmation.BankHolder.OrDbNull()
                };
                var BankAcctType = new SqlParameter
                {
                    ParameterName = "BankAcctType",
                    Value = paymentConfirmation.AccountType.OrDbNull()
                };
                var UpdatedPhone = new SqlParameter
                {
                    ParameterName = "UpdatedPhone",
                    Value = paymentConfirmation.UpdatedPhone.OrDbNull()
                };
                var UpdatedPhoneDate = new SqlParameter
                {
                    ParameterName = "UpdatedPhoneDate",
                    Value = paymentConfirmation.UpdatedPhoneDate.OrDbNull()
                    // Value = string.Empty
                };
                var SaveAccountFuture = new SqlParameter
                {
                    ParameterName = "SaveAccountFuture",
                    Value = paymentConfirmation.SaveAccountFuture.OrDbNull()
                };
                var BankAccountName = new SqlParameter
                {
                    ParameterName = "BankAccountName",
                    Value = paymentConfirmation.BankAccountName.OrDbNull()
                };

                var ScheduleMethod = new SqlParameter
                {
                    ParameterName = "ScheduleMethod",
                    Value = paymentConfirmation.AccountType.OrDbNull()
                };
                var Email = new SqlParameter
                {
                    ParameterName = "Email",
                    Value = paymentConfirmation.Email.OrDbNull()
                };
                var Filter = new SqlParameter
                {
                    ParameterName = "Filter",
                    Value = "SaveAchAutoPay"
                };
                var paymentFrequency = new SqlParameter
                {
                    ParameterName = "paymentFrequency",
                    Value = paymentConfirmation.paymentFrequency.OrDbNull()
                };
                var Message = new SqlParameter
                {
                    ParameterName = "Message",
                    //   SqlDbType = SqlDbType.VarChar,
                    //  Direction = System.Data.ParameterDirection.Output 
                    Value = string.Empty
                };
                var ScheduleID = new SqlParameter
                {
                    ParameterName = "ScheduleID",
                    Value = paymentConfirmation.ScheduleID.OrDbNull()
                };
                /// save Ach autopay  RECURRING PAYMENT
                if (paymentConfirmation.Status != "UpdateAutoPay")
                {
                    object[] parameters = new object[] { Confirmation, AcctNo, TranPayment, TranFee, BankABA, BankAcctNo, BankName, BankHolder, BankAcctType, UpdatedPhone, UpdatedPhoneDate, SaveAccountFuture, BankAccountName, ScheduleMethod, Email, Filter, paymentFrequency, Message };
                    var resultMessage = dataCollection.SaveAchAutoPaySetup(parameters);

                    if (resultMessage == "complete")
                    {
                        message = confirmation;
                    }
                    else
                    {
                        message = "You are trying to make the same payment again.";
                    }
                }
                else
                {
                    /// Update Ach autopay  RECURRING PAYMENT
                    object[] parameters = new object[] { Confirmation, AcctNo, TranPayment, TranFee, BankABA, BankAcctNo, BankName, BankHolder, BankAcctType, BankAccountName, ScheduleMethod, Email, paymentFrequency, ScheduleID, SaveAccountFuture, UpdatedPhone };
                    var resultMessage = dataCollection.UpdateAchAutoPaySetup(parameters);

                    if (resultMessage == "complete")
                    {
                       // message = confirmation;
                      //  message = "Your payment transaction has been completed.";
                        message = confirmation;
                    }
                    else
                    {
                        message = "You are trying to make the same payment again.";
                    }
                }
            }
            catch
            {
                throw;
            }
            return message;
        }

        /// <summary>
        ///Get Auto Pay Recurring
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public List<AutoPayRecurringEntity> GetAutoPayDetail(string accountNumber)
        {
            var autoPayDetail = new List<AutoPayRecurringEntity>();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };

                object[] parameters = new object[] { AcctNo };
                autoPayDetail = dataCollection.GetAutoPayDetail<AutoPayRecurringEntity>(parameters);
            }
            catch
            {
                throw;
            }
            return autoPayDetail;
        }

        /// <summary>
        ///Delete Auto Pay Recurring
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public string DeleteAutoPay(string AccountNumber)
        {
            string message = string.Empty;
            try
            {

                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = AccountNumber.OrDbNull()
                };
                var Message = new SqlParameter
                {

                    ParameterName = "Message",
                    DbType = System.Data.DbType.String,
                    Size = 100,
                    Direction = System.Data.ParameterDirection.Output
                };

                object[] parameters = new object[] { AcctNo, Message };
                var PaymentConfirmationUser = dataCollection.DeleteAutoPaySetup<PaymentConfirmationEntity>(parameters);

                object outParamValue = Convert.ToString(Message.Value);
                message = Convert.ToString(outParamValue);

            }
            catch 
            {
                throw;

            }


            return message;
        }

        // Get Duplicate Payment
        public List<DuplicatePaymentEntity> GetAutoPayDuplicatePayment(string accountNumber)
        {
            var duplicatePaymentEntity = new DuplicatePaymentEntity();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo };
                return dataCollection.GetAutoPayDuplicatePayment<DuplicatePaymentEntity>(parameters).ToList();

            }
            catch 
            {
                throw;
            }
        }

        public void SaveCardBillingAddress(CardInfoEntity cardInfo)
        {
            try
            {

                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = cardInfo.AccountNumber.OrDbNull()
                };
                var ConfirmationId = new SqlParameter
                {
                    ParameterName = "ConfirmationId",
                    Value = cardInfo.MerchantReferenceCode.OrDbNull()
                };

                var TokenId = new SqlParameter
                {
                    ParameterName = "TokenId",
                    Value = cardInfo.TokenId.OrDbNull()
                };

                var FirstName = new SqlParameter
                {
                    ParameterName = "FirstName",
                    Value = cardInfo.FirstName.OrDbNull()
                };
                var LastName = new SqlParameter
                {
                    ParameterName = "LastName",
                    Value = cardInfo.LastName.OrDbNull()
                };
                var PrimaryNumber = new SqlParameter
                {
                    ParameterName = "PrimaryNumber",
                    Value = cardInfo.PrimaryNumber.OrDbNull()
                };
                var EmailID = new SqlParameter
                {
                    ParameterName = "EmailID",
                    Value = cardInfo.Email.OrDbNull()
                };
                var Address = new SqlParameter
                {
                    ParameterName = "Address",
                    Value = cardInfo.Street.OrDbNull()
                };
                var City = new SqlParameter
                {
                    ParameterName = "City",
                    Value = cardInfo.City.OrDbNull()
                };
                var State = new SqlParameter
                {
                    ParameterName = "State",
                    Value = cardInfo.State.OrDbNull()
                };
                var Zip = new SqlParameter
                {
                    ParameterName = "Zip",
                    Value = cardInfo.PostelCode.OrDbNull()
                };
                var Country = new SqlParameter
                {
                    ParameterName = "Country",
                    Value = cardInfo.State.OrDbNull()
                };

                object[] parameters = new object[] { TokenId, ConfirmationId, AcctNo, FirstName, LastName, PrimaryNumber, EmailID, Address, City, State, Zip, Country };
                dataCollection.SaveCardBillingAddress(parameters);
            }
            catch 
            {
                throw;

            }
        }

        public void InsertAutoPayCardSchedule(CardInfoEntity cardInfo)
        {
            try
            {
                var ConfirmationId = new SqlParameter
                {
                    ParameterName = "ConfirmationId",
                    Value = cardInfo.MerchantReferenceCode.OrDbNull()
                };

                var Acctno = new SqlParameter
                {
                    ParameterName = "Acctno",
                    Value = cardInfo.AccountNumber.OrDbNull()
                };

                var TokenId = new SqlParameter
                {
                    ParameterName = "TokenId",
                    Value = cardInfo.TokenId.OrDbNull()
                };
                var CardName = new SqlParameter
                {
                    ParameterName = "CardName",
                    Value = cardInfo.CardName.OrDbNull()
                };
                var CardType = new SqlParameter
                {
                    ParameterName = "CardType",
                    Value = cardInfo.CardType == "001" ? "V" : "M"
                };
                var CardNumber = new SqlParameter
                {
                    ParameterName = "CardNumber",
                    Value = cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4)
                };
                var CardExpiry = new SqlParameter
                {
                    ParameterName = "CardExpiry",
                    Value = cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear.OrDbNull()
                };
                var Amount = new SqlParameter
                {
                    ParameterName = "Amount",
                    Value = cardInfo.Amount.OrDbNull()
                };

                var paymentFrequency = new SqlParameter
                {
                    ParameterName = "paymentFrequency",
                    Value = cardInfo.paymentFrequency.OrDbNull()
                };
                var TranFee = new SqlParameter
                {
                    ParameterName = "TranFee",
                    Value = cardInfo.Fee.OrDbNull()
                };

                var ScheduleID = new SqlParameter
                {
                    ParameterName = "ScheduleID",
                    Value = cardInfo.ScheduleID.OrDbNull()
                };

                /// save card autopay  RECURRING PAYMENT
                if (cardInfo.Status != "UpdateAutoPay")
                {
                    object[] parameters = new object[] { ConfirmationId, Acctno, TokenId, CardName, CardType, CardNumber, CardExpiry, Amount, paymentFrequency, TranFee };
                    dataCollection.InsertAutoPayCardSchedule(parameters);
                }
                else
                {
                    // update card autopay RECURRING PAYMENT
                    object[] parameters = new object[] { ConfirmationId, Acctno, TokenId, CardName, CardType, CardNumber, CardExpiry, Amount, paymentFrequency, TranFee, ScheduleID };
                    dataCollection.UpdateAutoPayCardSchedule(parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SaveCardTransactionLog(CardDetailsEntity cardInfo)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = cardInfo.AcctNo.OrDbNull()
                };
                var ConfirmationId = new SqlParameter
                {
                    ParameterName = "ConfirmationId",
                    Value = cardInfo.ConfirmationId.OrDbNull()
                };
                var TokenId = new SqlParameter
                {
                    ParameterName = "TokenId",
                    Value = cardInfo.TokenId.OrDbNull()
                };
                var RequestId = new SqlParameter
                {
                    ParameterName = "RequestId",
                    Value = cardInfo.RequestId.OrDbNull()
                };
                var CardName = new SqlParameter
                {
                    ParameterName = "CardName",
                    Value = cardInfo.CardName.OrDbNull()
                };
                var CardNumber = new SqlParameter
                {
                    ParameterName = "CardNumber",
                    Value = cardInfo.CardNumber.OrDbNull()
                };
                var CardExpiry = new SqlParameter
                {
                    ParameterName = "CardExpiry",
                    Value = cardInfo.CardExpiry.OrDbNull()
                };
                var CardType = new SqlParameter
                {
                    ParameterName = "CardType",
                    Value = cardInfo.CardType == "001" ? "V" : "M".OrDbNull()
                };
                var ReasonCode = new SqlParameter
                {
                    ParameterName = "ReasonCode",
                    Value = cardInfo.ReasonCode.OrDbNull()
                };
                var Description = new SqlParameter
                {
                    ParameterName = "Description",
                    Value = cardInfo.Description.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo, ConfirmationId, TokenId, RequestId, CardName, CardNumber, CardExpiry, CardType, ReasonCode, Description };
                dataCollection.SaveCardTransactionLog(parameters);
            }
            catch
            {
                throw;
            }
        }
    }
}
