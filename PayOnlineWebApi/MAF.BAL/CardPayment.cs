namespace MAF.BAL
{
    using MAF.DAL;
    using MAF.ENTITY;
    using MAF.ENTITY.CyberSource;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class CardPayment : ICardPayment
    {
        string message = string.Empty;
        private IDataCollection dataCollection = null;        
        public CardPayment()
        {
            this.dataCollection = new DataCollection();
        }

        public List<CardDetailsEntity> CardDetails(string accountNumber)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo };
                var cardDetailsLIst = dataCollection.CardDetails<CardDetailsEntity>(parameters);
                return cardDetailsLIst;
            }
            catch
            {
                throw;
            }
        }

        public string SaveCardInfo(CardInfoEntity cardInfo)
        {
            try
            {
                var ConfirmationId = new SqlParameter
                {
                    ParameterName = "ConfirmationId",
                    Value = cardInfo.MerchantReferenceCode.OrDbNull()
                };
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
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
                    Value = cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear
                };

                var FuturePaymentDate = new SqlParameter
                {
                    ParameterName = "FuturePaymentDate",
                    Value = cardInfo.FuturePaymentDate.OrDbNull()
                };
                var Fee = new SqlParameter
                {
                    ParameterName = "Fee",
                    Value = cardInfo.Fee.OrDbNull()
                };
                var Status = new SqlParameter
                {
                    ParameterName = "Status",
                    Value = cardInfo.Status.OrDbNull()
                };
                var Amount = new SqlParameter
                {
                    ParameterName = "Amount",
                    Value = (Convert.ToDecimal(cardInfo.Amount) - Convert.ToDecimal(cardInfo.Fee))
                };

                object[] parameters = new object[] { ConfirmationId, AcctNo, TokenId, CardName, CardType, CardNumber, CardExpiry, FuturePaymentDate, Fee, Status, Amount };
                var CardPaymentConfirmationUser = dataCollection.CardSavePayment(parameters);                              
            }
            catch
            {
                throw;
            }
            return message;
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
                    Value = "United States"
                };

                object[] parameters = new object[] { TokenId, ConfirmationId, AcctNo, FirstName, LastName, PrimaryNumber, EmailID, Address, City, State, Zip, Country };
                dataCollection.SaveCardBillingAddress(parameters);
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

                object[] parameters = new object[] { AcctNo, ConfirmationId, TokenId, RequestId,CardName,CardNumber,CardExpiry,CardType, ReasonCode, Description };
                dataCollection.SaveCardTransactionLog(parameters);
            }
            catch
            {
                throw;
            }
        }

        public string CheckCardMaxLimit(CardInfoEntity cardInfo)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = cardInfo.AccountNumber.OrDbNull()
                };
                var CardNumber = new SqlParameter
                {
                    ParameterName = "CardNumber",
                    Value = cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4).OrDbNull()
                };
              
                var CardExpiry = new SqlParameter
                {
                    ParameterName = "CardExpiry",
                    Value = cardInfo.ExpirationMonth+"/"+cardInfo.ExpirationYear.OrDbNull()
                };
                var CardName = new SqlParameter
                {
                    ParameterName = "CardName",
                    Value = cardInfo.CardName.OrDbNull()
                };
                  var TokenId = new SqlParameter
                {
                    ParameterName = "TokenId",
                    Value = cardInfo.TokenId.OrDbNull()
                };
                  object[] parameters = new object[] {AcctNo, CardNumber, CardName, CardExpiry, TokenId };
                  return dataCollection.CheckCardMaxLimit<CardDetailsEntity>(parameters).First().CardMaxLimit;                                                  
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCard(string tokenIdValue)
        {
            try
            {
                var tokenId = new SqlParameter
                {
                    ParameterName = "tokenId",
                    Value = tokenIdValue.OrDbNull()
                };
                object[] parameters = new object[] { tokenId };
                dataCollection.DeleteCard(parameters);
            }
            catch 
            {
                throw;
            }
        }

        public BillingAddressEntity GetBillingAddress(string accountNumber,string tokenId)
        {
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                var TokenId = new SqlParameter
                {
                    ParameterName = "TokenId",
                    Value = tokenId.OrDbNull()
                };

                object[] parameters = new object[] { AcctNo, TokenId };
                return dataCollection.GetBillingAddress<BillingAddressEntity>(parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CheckValidCardTransaction(string accountNumber, string amount, string futurePaymentDate, bool IsTotalAmountDue)
        {
            try
            {
                Utility objUtil = new Utility();
                IPayOnline payOnline = new PayOnline();
                var NSFStatus = payOnline.CheckNSFStatus(accountNumber);
                if (NSFStatus == "ABlocked")
                {
                    return ResourceFile.Common.LoginAcceptedPaymentsFaild;
                }
                else
                {
                    return objUtil.ValidateTransaction(accountNumber, Convert.ToDecimal(amount), futurePaymentDate, IsTotalAmountDue);
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<CardInfoEntity> GetScheduledTransactionsForCardPayment()
        {
            try
            {
                return dataCollection.GetScheduledTransactionsForCardPayment<CardInfoEntity>().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateScheduleTransactionStatus(string scheduleDetailsId,string status)
        {
            try
            {
                var ScheduleDetailsId = new SqlParameter
                {
                    ParameterName = "ScheduleDetailsId",
                    Value = scheduleDetailsId.OrDbNull()
                };
                var Status = new SqlParameter
                {
                    ParameterName = "Status",
                    Value = status.OrDbNull()
                };

                object[] parameters = new object[] { ScheduleDetailsId, Status };
                dataCollection.UpdateScheduleTransactionStatus(parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void UpdateMAFAccountConsecutiveNSFStatus()
        {
            try
            {

                dataCollection.UpdateMAFAccountConsecutiveNSFStatus();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

   

    }
}
