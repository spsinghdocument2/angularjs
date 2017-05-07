using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class LoanPayment : ILoanPayment
    {
        private IDataCollection idata = null;
        public LoanPayment()
        {
            this.idata = new DataCollection();
        }

        #region Get LoanPayment
        public LoanPaymentEntity GetLoanPayment(string accountNumber)
        {
            
                var loanPaymentEntity = new LoanPaymentEntity();
                try
                {
                    var AcctNo = new SqlParameter
                    {
                        ParameterName = "AcctNo",
                        Value = accountNumber.OrDbNull()
                    };

                    object[] parameters = new object[] { AcctNo };



                var loanPayment = idata.LoanPayment<LoanPaymentEntity>(parameters).FirstOrDefault();
                if (loanPayment != null)
                {
                    loanPaymentEntity.AcctCurrentBal = loanPayment.AcctCurrentBal;
                    loanPaymentEntity.AcctDaysPastDue = loanPayment.AcctDaysPastDue;
                    loanPaymentEntity.AcctPastDueAmt = loanPayment.AcctPastDueAmt;
                    loanPaymentEntity.ConfirmationID = loanPayment.ConfirmationID;
                    loanPaymentEntity.LastTransactionDate = (loanPayment.TranDate != null ? String.Format("{0:MM/dd/yyyy}", loanPayment.TranDate) : string.Empty);
                    loanPaymentEntity.TranPayment = loanPayment.TranPayment;
                    loanPaymentEntity.TranFee = loanPayment.TranFee;
                    loanPaymentEntity.BankABA = loanPayment.BankABA;
                    loanPaymentEntity.BankAcctNo = loanPayment.BankAcctNo;
                    loanPaymentEntity.BankName = loanPayment.BankName;
                    loanPaymentEntity.ProfilePicture = loanPayment.ProfilePicture;
                    loanPaymentEntity.BankHolder = loanPayment.BankHolder;
                    loanPaymentEntity.BankAcctType = loanPayment.BankAcctType;
                    loanPaymentEntity.AcctFreq = loanPayment.AcctFreq;
                    loanPaymentEntity.AcctNextDueDate = loanPayment.TranDate;
                    loanPaymentEntity.ActionLogSubType = loanPayment.ActionLogSubType;
                    loanPaymentEntity.NextDueDate = (loanPayment.AcctNextDueDate != null ? String.Format("{0:MM/dd/yyyy}", loanPayment.AcctNextDueDate) : string.Empty);
                   
              
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return loanPaymentEntity;
        }

        #endregion

        #region Duplicate payment
        // Get Duplicate Payment
        public List<DuplicatePaymentEntity> GetDuplicatePayment(string accountNumber)
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
                return idata.DuplicatePayment<DuplicatePaymentEntity>(parameters).ToList();                
                             
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Method to get alerts based on account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        /// <returns>List of Alerts</returns>
        public List<AlertEntity> GetAlerts(string accountNumber)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                object[] parameters = new object[] { AcctNo };
                var alerts = idata.GetAlerts<AlertEntity>(parameters);
                return alerts;
            }
            catch
            {
                throw;
            }
        }
    }
}
