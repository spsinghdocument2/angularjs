using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class PayOnline : IPayOnline
    {
        private IDataCollection dataCollection = null;

        public PayOnline()
        {
            this.dataCollection = new DataCollection();
        }

        public PayonlineEntity GetAccountInformation(string accountNumber)
        {
            var payonlineEntity = new PayonlineEntity();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo };
                var account = dataCollection.GetAccountDetails<AccountEntity>(parameters).FirstOrDefault();
                if (account != null)
                {
                    payonlineEntity.AccountNumber = account.AcctNo;
                    payonlineEntity.AccountHolder = account.FullName.ToUpper();
                    payonlineEntity.CurrentBalance = String.Format("{0:c}", account.AcctCurrentBal);   
                    payonlineEntity.AmountPastDue = String.Format("{0:c}", account.AcctPastDueAmt); 
                    payonlineEntity.RegularAmountDue = String.Format("{0:c}", account.AcctRegPayAmt);
                    payonlineEntity.OtherAmount = String.Format("{0:f}", account.AcctRegPayAmt);
                    payonlineEntity.TotalAmountDue = String.Format("{0:f}", account.AcctPastDueAmt);
                    payonlineEntity.AccountHolderPhoneNumber = account.UpdatedPhone;
                    payonlineEntity.DueDate = account.AcctNextDueDate != null ? String.Format("{0:MM/dd/yyyy}", account.AcctNextDueDate) : string.Empty;
                    payonlineEntity.LastBankAccountType = Convert.ToString(account.BankAcctType);
                    payonlineEntity.LastTransactionFee = Convert.ToDecimal(account.TranFee).ToString();
                    payonlineEntity.LastTransactionPayment = Convert.ToDecimal(account.TranPayment).ToString();
                    payonlineEntity.TotalPayoffAmmount = String.Format("{0:f}", account.AcctPayOff);
                    payonlineEntity.RemainingTerm = Convert.ToString(account.RemainingTerm);
                    payonlineEntity.TodayDate = DateTime.Now.ToString("MM/dd/yyyy");
                    payonlineEntity.AcctDaysPastDue = account.AcctDaysPastDue;
                }
            }
            catch
            {
                throw;
            }
            return payonlineEntity;
        }

        public List<PaymentHistoryEntity> GetPaymentHistory(string accountNumber)
        {
            var AcctNo = new SqlParameter
            {
                ParameterName = "AcctNo",
                Value = accountNumber.OrDbNull()
            };
            object[] parameters = new object[] { AcctNo };

            var paymentHistory = dataCollection.PaymentHistory<PaymentHistoryEntity>(parameters);
            try
            {
                Global.Instance.holder.AddOrUpdate(Convert.ToInt32(accountNumber), paymentHistory, (k, v1) => paymentHistory);
            }
            catch 
            {
                throw;
            }
            return paymentHistory;

        }

        public List<PaymentHistoryEntity> SearchPaymentHistory(SearchPaymentEntity searchPayment)
        {
            var accountNumber = searchPayment.AccountNumber;
            var paymentHistory = new List<PaymentHistoryEntity>();
            try
            {                
                DateTime fromDate = Convert.ToDateTime(searchPayment.FromDate); //.ToString("yyyyMMddHHmmss");

                DateTime toDate = Convert.ToDateTime(searchPayment.ToDate);

                if (Global.Instance.holder.ContainsKey(Convert.ToInt32(accountNumber)))
                {
                    paymentHistory = Global.Instance.holder[Convert.ToInt32(accountNumber)].Where(a => Convert.ToDateTime(a.TranDate).Date >= fromDate.Date && Convert.ToDateTime(a.TranDate).Date <= toDate.Date).ToList();                    
                }
            }
            catch 
            {
                throw;
            }
            return paymentHistory;
        }

        public List<SearchPaymentDetailsEntity> SearchPaymentDetails(string accountNumber, SearchPaymentEntity searchPayment)
        {
            var paymentDetails = new List<SearchPaymentDetailsEntity>();            
            try
            {
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    var Acctno = new SqlParameter
                    {
                        ParameterName = "Acctno",
                        Value = accountNumber.OrDbNull()
                    };

                    object[] parameters = new object[] { Acctno };
                    paymentDetails = dataCollection.GetSearchPayment<SearchPaymentDetailsEntity>(parameters);
                    Global.Instance.searchPaymentHolder.AddOrUpdate(Convert.ToInt32(accountNumber), paymentDetails, (k, v1) => paymentDetails);
                }
                else
                {
                    DateTime fromDate = Convert.ToDateTime(searchPayment.FromDate); //.ToString("yyyyMMddHHmmss");
                    DateTime toDate = Convert.ToDateTime(searchPayment.ToDate);

                    if (Global.Instance.searchPaymentHolder.ContainsKey(Convert.ToInt32(searchPayment.AccountNumber)))
                    {
                        paymentDetails = Global.Instance.searchPaymentHolder[Convert.ToInt32(searchPayment.AccountNumber)].Where(a => Convert.ToDateTime(a.ScheduleDate).Date >= fromDate.Date && Convert.ToDateTime(a.ScheduleDate).Date <= toDate.Date).ToList();
                        //  paymentHistory = Global.Instance.holder[Convert.ToInt32(accountNumber)].Where(a => Convert.ToDateTime(a.TranDate.is).Is).ToList();
                    }
                }

            }
            catch
            {
                throw;
            }
            return paymentDetails;
        }

        public void CancelSchedulePayment(int id)
        {
            try
            {
                var Id = new SqlParameter
                {
                    ParameterName = "Id",
                    Value = id
                };
                object[] parameters = new object[] { Id };
                dataCollection.CancelSchedulePayment(parameters);
            }
            catch 
            {
                throw;
            }
        }

        public PaymentHistoryEntity LastPaymentHistory(string accountNumber)
        {
            var lastPaymentHistoryEntity = new PaymentHistoryEntity();
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                object[] parameters = new object[] { AcctNo };
                var lastPaymen = dataCollection.LastPaymen<PaymentHistoryEntity>(parameters);
                if (lastPaymen.Count > 0)
                {

                    lastPaymentHistoryEntity.ID = lastPaymen.Select(a => a.ID).FirstOrDefault();
                    lastPaymentHistoryEntity.ConfirmationID = lastPaymen.Select(a => a.ConfirmationID).First().ToString();
                    lastPaymentHistoryEntity.TranDate = lastPaymen.Select(a => a.TranDate).First().ToString();
                    lastPaymentHistoryEntity.TranPayment = lastPaymen.Select(a => a.TranPayment).First().ToString();
                    lastPaymentHistoryEntity.TranFee = lastPaymen.Select(a => a.TranFee).First().ToString();                    
                    lastPaymentHistoryEntity.BankAcctNo = lastPaymen.Select(a => a.BankAcctNo).First().ToString();                   
                    lastPaymentHistoryEntity.BankHolder = lastPaymen.Select(a => a.BankHolder).First().ToString();
                    lastPaymentHistoryEntity.BankAcctType = lastPaymen.Select(a => a.BankAcctType).First().ToString();
                    lastPaymentHistoryEntity.Status = lastPaymen.Select(a => a.Status).First().ToString();

                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return lastPaymentHistoryEntity;
        }

        public string CheckNSFStatus(string accountNumber)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };
                object[] parameters = new object[] { AcctNo };
                return dataCollection.CheckNSFStatus<PayonlineEntity>(parameters).First().NSFStatus;
            }
            catch
            {
                throw;
            }
        }
    }
}
