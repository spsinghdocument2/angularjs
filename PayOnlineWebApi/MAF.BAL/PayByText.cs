using MAF.DAL;
using MAF.ENTITY;
using MAF.ENTITY.CyberSource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class PayByText : IPayByText
    {
        private IDataCollection idata = null;
        public PayByText()
        {
            this.idata = new DataCollection();
        }

        public string SavePayByTextWithACH(PaymentConfirmationEntity paymentConfirmation)
        {
            string message = string.Empty;
            string result = string.Empty;
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
                    Value = paymentConfirmation.BankAccountName.OrDbNull()
                };
                var BankAcctType = new SqlParameter
                {
                    ParameterName = "BankAcctType",
                    Value = paymentConfirmation.AccountType.OrDbNull()
                };
                var CreatedBy = new SqlParameter
                {
                    ParameterName = "CreatedBy",
                    Value = paymentConfirmation.BankHolder.OrDbNull()
                };
                var Phone = new SqlParameter
                {
                    ParameterName = "Phone",
                    Value = paymentConfirmation.UpdatedPhone.OrDbNull()
                };
                var Email = new SqlParameter
                {
                    ParameterName = "Email",
                    Value = paymentConfirmation.Email.OrDbNull()
                };
                var textNumber = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = paymentConfirmation.TextNumber.OrDbNull()
                };

                if (paymentConfirmation.EditStatus.Equals("True"))
                {
                    object[] parameters = new object[] { Confirmation, AcctNo, BankABA, BankAcctNo, BankName, BankHolder, BankAcctType, CreatedBy, Phone, Email, textNumber };
                    result = idata.UpdatePayByTextACHSetup(parameters);
                }
                else
                {
                    object[] parameters = new object[] { Confirmation, AcctNo, BankABA, BankAcctNo, BankName, BankHolder, BankAcctType, CreatedBy, Phone, Email, textNumber };
                    result = idata.SavePayByTextACHSetup(parameters);
                }

                if (result == "complete")
                {
                    message = confirmation;
                }
                else
                {
                    message = "Something went wrong.";
                }
            }
            catch
            {
                throw;
            }
            return message;
        }


        public string SavePayByTextWithCard(CardInfoEntity cardInfo, string createdBy)
        {
            string message = string.Empty;
            string result = string.Empty;
            try
            {
                var ConfirmationId = new SqlParameter
                {
                    ParameterName = "Confirmation",
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
                    Value = cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4).OrDbNull()
                };
                var CardExpiry = new SqlParameter
                {
                    ParameterName = "CardExpiry",
                    Value = cardInfo.ExpirationMonth + "/" + cardInfo.ExpirationYear.OrDbNull()
                };

                var createdByUser = new SqlParameter
                {
                    ParameterName = "CreatedBy",
                    Value = createdBy.OrDbNull()
                };
                var textNumber = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = cardInfo.TextNumber.OrDbNull()
                };

                if (cardInfo.EditStatus.Equals("True"))
                {
                    object[] parameters = new object[] { ConfirmationId, AcctNo, TokenId, CardName, CardType, CardNumber, CardExpiry, createdByUser, textNumber };
                    result = idata.UpdatePayByTextCardSetup(parameters);
                }
                else
                {
                    object[] parameters = new object[] { ConfirmationId, AcctNo, TokenId, CardName, CardType, CardNumber, CardExpiry, createdByUser, textNumber };
                    result = idata.SavePayByTextCardSetup(parameters);
                }

                if (result == "complete")
                {
                    message = cardInfo.MerchantReferenceCode;
                }
                else
                {
                    message = "Something went wrong!";
                }
            }
            catch
            {
                throw;
            }
            return message;
        }

        public string DeletePayByTextSetup(string accountNumber)
        {
            string message = string.Empty;
            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };

                object[] parameters = new object[] {  AcctNo };
                var result = idata.DeletePayByTextSetup(parameters);

                if (result == "complete")
                {
                    message = "Pay By Text Setup is deleted successfully.";
                }
                else
                {
                    message = "Something went wrong!";
                }
            }
            catch
            {
                throw;
            }
            return message;
        }

        public PayByTextEntity GetPayByTextSetup(string accountNumber)
        {
            try
            {
                var accountNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber
                };

                object[] parameters = new object[] { accountNo };
                return idata.GetPayByTextSetupDetail<PayByTextEntity>(parameters).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }


        public List<PayByTextEntity> GetActivePayByTextSetup()
        {
            try
            {
                return idata.GetActivePayByText<PayByTextEntity>();
            }
            catch
            {
                throw;
            }
        }

        public string SavePayByTextMessageLog(PayByTextMessageLogEntity objEntity)
        {
            try
            {
                var acctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = objEntity.AcctNo.OrDbNull()
                };

                var textNumber = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = objEntity.TextNumber.OrDbNull()
                };
                var messsage = new SqlParameter
                {
                    ParameterName = "Messsage",
                    Value = objEntity.Message.OrDbNull()
                };
                var type = new SqlParameter
                {
                    ParameterName = "Type",
                    Value = objEntity.Type.OrDbNull()
                };
                var keyword = new SqlParameter
                {
                    ParameterName = "Keyword",
                    Value = objEntity.Keyword.OrDbNull()
                };

                    object[] parameters = new object[] { acctNo, textNumber, messsage, type, keyword};
                    return idata.SavePayByTextMessageLog(parameters);
            }
            catch
            {
                throw;
            }
        }

        public List<PayByTextMessageLogEntity> GetPayByTextMessageLog(string accountNumber, string textNumber)
        {
            try
            {
                var accountNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                var textNo = new SqlParameter
                {
                    ParameterName = "TextNumber",
                    Value = textNumber.OrDbNull()
                };

                object[] parameters = new object[] { accountNo, textNo };
                return idata.GetPayByTextMessageLog<PayByTextMessageLogEntity>(parameters);
            }
            catch
            {
                throw;
            }
        }
    }
}
