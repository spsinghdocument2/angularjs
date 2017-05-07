namespace MAF.BAL
{
    using MAF.DAL;
    using MAF.ENTITY;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class AchPayment : IAchPayment
    {
        private IDataCollection dataCollection = null;
        public AchPayment()
        {
            this.dataCollection = new DataCollection();
        }
        /// <summary>
        ///Get saved ach account details
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public List<AchDetailEntity> GetSavedAchDetail(string accountNumber)
        {
            var AchDetail = new List<AchDetailEntity>();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo };

                AchDetail = dataCollection.GetAchDetail<AchDetailEntity>(parameters);
            }
            catch
            {
                throw;
            }

            return AchDetail;
        }

        /// <summary>
        /// Delete saved ach detail
        /// </summary>
        /// <param name="bankAccountNumber">Id/param>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public string DeleteSavedAchDetail(long id, int accountNumber)
        {
            string message = string.Empty;

            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };
                var ID = new SqlParameter
                {
                    ParameterName = "ID",
                    Value = id
                };

                object[] parameters = new object[] { AcctNo, ID };
                message = dataCollection.DeleteSavedAch(parameters);
            }
            catch
            {
                throw;
            }

            return message;
        }

        /// <summary>
        /// Method to do ACH Future Pay
        /// </summary>
        /// <param name="paymentConfirmation"></param>
        /// <returns></returns>
        public string SavePayment(PaymentConfirmationEntity paymentConfirmation)
        {
            string message = string.Empty, confirmation=string.Empty;
            if (!string.IsNullOrEmpty(paymentConfirmation.ConfirmationId))
                confirmation = paymentConfirmation.ConfirmationId;
            else
             confirmation = String.Concat(DateTime.Now.ToString("yy"), DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'), DateTime.Now.ToString("HHssf"));

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
                var FuturePaymentDate = new SqlParameter
                {
                    ParameterName = "FuturePaymentDate",
                    Value = paymentConfirmation.FuturePaymentDate.OrDbNull()
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

                object[] parameters = new object[] { Confirmation, AcctNo, TranPayment, TranFee, BankABA, BankAcctNo, BankName, BankHolder, BankAcctType, UpdatedPhone, UpdatedPhoneDate, SaveAccountFuture, BankAccountName, FuturePaymentDate, ScheduleMethod, Email };
                var resultMessage = dataCollection.SaveAchPayment(parameters);

                if (resultMessage == "complete")
                {
                    if (string.IsNullOrEmpty(paymentConfirmation.FuturePaymentDate))
                    {
                        SendMail.Instance.PaymentConfirmation((Convert.ToDecimal(paymentConfirmation.PaymentAmount) + Convert.ToDecimal(paymentConfirmation.FeeAmoun)).ToString(), confirmation, paymentConfirmation.Email, paymentConfirmation.BankHolder);
                    }
                    message = confirmation;
                    // message = "Your payment transaction has been completed.";
                }
                else
                {
                    message = "Please try again!";
                }
            }
            catch
            {
                throw;
            }
            return message;
        }

        //Validation for Payment Transaction
        public RoutingNumberEntity ValidatePayment(AccountInformationEntity accountInformation)
        {
            var routingNumberEntity = new RoutingNumberEntity();
            IPayOnline payOnline = new PayOnline();         
            try
            {
                var NSFStatus = payOnline.CheckNSFStatus(accountInformation.AccountNumber);
                if (NSFStatus == "ABlocked" || NSFStatus == "Blocked")
                {
                    routingNumberEntity.Error= ResourceFile.Common.LoginAcceptedPaymentsFaild;
                }
                string validatePayment = ValidateTransactionDetails(accountInformation.AccountNumber, accountInformation.Amount, accountInformation.CheckingAccountNumber,accountInformation.FuturePaymentDate,Convert.ToBoolean(accountInformation.IsTotalAmountDue));
                if (validatePayment == "true")
                {
                   return routingNumberEntity = ValidateRoutingNumber(accountInformation.Routing);
                }
                else
                {
                    routingNumberEntity.Error = validatePayment;
                    return routingNumberEntity;
                }
            }
            catch
            {
                throw;
            }
        }



        protected string ValidateTransactionDetails(string accountNumber, string amount, string bankAcount, string futurePaymentDate, bool IsTotalAmountDue)
        {                        
            try
            {
                Utility objUtil = new Utility();
                if (ValidateAccountNumberByChecksum(bankAcount))
                {
                    return "Invalid checking account number. <BR/>Please enter your checking account number as it appears on your checks.";
                }
                else
                {
                    return objUtil.ValidateTransaction(accountNumber, Convert.ToDecimal(amount), futurePaymentDate, IsTotalAmountDue);
                }
            }
            catch (Exception)
            {
                 throw;
            }            
        }

        private bool ValidateAccountNumberByChecksum(string id)
        { //// check whether input string is null or empty 
            try
            {
                if ((string.IsNullOrEmpty(id)) || (id.Length < 15))
                {
                    return false;
                }
                int idLength = id.Length;
                int currentDigit;
                int idSum = 0;
                int currentProcNum = 0; //the current process number (to calc odd/even proc)

                for (int i = idLength - 1; i >= 0; i--)
                {
                    //get the current rightmost digit from the string
                    string idCurrentRightmostDigit = id.Substring(i, 1);

                    //parse to int the current rightmost digit, if fail return false (not-valid id)
                    if (!int.TryParse(idCurrentRightmostDigit, out currentDigit))
                        return false;

                    //double value of every 2nd rightmost digit (odd)
                    //if value 2 digits (can be 18 at the current case),
                    //then sumarize the digits (made it easy the by remove 9)
                    if (currentProcNum % 2 != 0)
                    {
                        if ((currentDigit *= 2) > 9)
                            currentDigit -= 9;
                    }
                    currentProcNum++; //increase the proc number

                    //summarize the processed digits
                    idSum += currentDigit;
                }

                //if digits sum is exactly divisible by 10, return true (valid), else false (not-valid)
                return (idSum % 10 == 0);
            }
            catch
            {
                throw;
            }
        }

        public RoutingNumberEntity ValidateRoutingNumber(string routingNumber)
        {
            var routingNumberEntity = new RoutingNumberEntity();
            try
            {
                if (ValidateRoutingNumberByChecksum(routingNumber))
                {
                    try
                    {
                        var ABA = new SqlParameter
                        {
                            ParameterName = "ABA",
                            Value = routingNumber.OrDbNull()
                        };

                        object[] parameters = new object[] { ABA };

                        var routingNumberDetail = dataCollection.VerifyRoutingNumber<RoutingNumberEntity>(parameters).FirstOrDefault();
                        if (routingNumberDetail != null)
                        {
                            routingNumberEntity.BankName = routingNumberDetail.BankName;
                        }
                        else
                        {
                            routingNumberEntity.ErrorRouting = "Invalid Routing Number";
                        }
                    }
                    catch
                    {
                        routingNumberEntity.Error = "Account can't be verified";
                    }
                }
                else
                {
                    routingNumberEntity.ErrorRouting = "Invalid Routing Number";
                }
            }
            catch
            {
                throw;
            }
            return routingNumberEntity;
        }

        private bool ValidateRoutingNumberByChecksum(string aba)
        {
            int m1, m2, m3, m4, m5, m6, m7, m8, m9, mTotal;
            double mSubValue;
            double mCheckDigit;
            try
            {
                if (aba.Length == 9)
                {
                    //http://www.quentinsagerconsulting.com/utilities/aba_online.php
                    m1 = Convert.ToInt32(aba.Substring(0, 1)) * 3;
                    m2 = Convert.ToInt32(aba.Substring(1, 1)) * 7;
                    m3 = Convert.ToInt32(aba.Substring(2, 1)) * 1;
                    m4 = Convert.ToInt32(aba.Substring(3, 1)) * 3;
                    m5 = Convert.ToInt32(aba.Substring(4, 1)) * 7;
                    m6 = Convert.ToInt32(aba.Substring(5, 1)) * 1;
                    m7 = Convert.ToInt32(aba.Substring(6, 1)) * 3;
                    m8 = Convert.ToInt32(aba.Substring(7, 1)) * 7;
                    m9 = Convert.ToInt32(aba.Substring(8, 1));

                    mTotal = m1 + m2 + m3 + m4 + m5 + m6 + m7 + m8;
                    mSubValue = mTotal % 10;

                    if (mSubValue > 0)
                        mSubValue = (Convert.ToInt32(mTotal / 10) * 10) + 10;
                    else
                        mSubValue = (Convert.ToInt32(mTotal / 10) * 10);

                    mCheckDigit = mSubValue - mTotal;

                    if (mCheckDigit == m9)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public List<AchDetailEntity> GetScheduledTransactionsForAchPayment()
        {
            try
            {
                return dataCollection.GetScheduledTransactionsForAchPayment<AchDetailEntity>().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
