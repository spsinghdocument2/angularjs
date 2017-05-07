using MAF.DAL;
using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class InsuranceInformation : IInsuranceInformation
    {
          private IDataCollection dataCollection = null;
          public InsuranceInformation()
          {
              this.dataCollection = new DataCollection();
          }


        #region Insurance Information post
        /// <summary>
        /////Save Insurance Information  function
        /// </summary>
        /// <param name="InsuranceInformationEntity">InsuranceInformationEntity</param>
        /// <returns>string</returns>
        public string SaveInsuranceInformation(InsuranceInformationEntity insuranceInformation)
        {
            string message = string.Empty;
            string path = insuranceInformation.InsuranceCardPath == null ? string.Empty : insuranceInformation.InsuranceCardPath;

            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = insuranceInformation.AccountNumber.OrDbNull()
                };
                var InsPolicyNo = new SqlParameter
                {
                    ParameterName = "InsPolicyNo",
                    Value = insuranceInformation.PolicyNumber.OrDbNull()
                };
                var InsEffDate = new SqlParameter
                {
                    ParameterName = "InsEffDate",
                    Value = insuranceInformation.EffectiveDate.OrDbNull()
                };
                var InsExpDate = new SqlParameter
                {
                    ParameterName = "InsExpDate",
                    Value = insuranceInformation.ExpirationDate.OrDbNull()
                };
                var InsuranceCompanyName = new SqlParameter
                {
                    ParameterName = "InsuranceCompanyName",
                    Value = insuranceInformation.InsuranceCompanyName.OrDbNull()
                };
                var InsuranceCompanyAddress = new SqlParameter
                {
                    ParameterName = "InsuranceCompanyAddress",
                    Value = insuranceInformation.InsuranceAddress.OrDbNull()
                };
                var InsuranceCompanyCity = new SqlParameter
                {
                    ParameterName = "InsuranceCompanyCity",
                    Value = insuranceInformation.InsuranceCity.OrDbNull()
                };
                var InsuranceCompanyState = new SqlParameter
                {
                    ParameterName = "InsuranceCompanyState",
                    Value = insuranceInformation.InsuranceState.OrDbNull()
                };
                var InsuranceCompanyZip = new SqlParameter
                {
                    ParameterName = "InsuranceCompanyZip",
                    Value = insuranceInformation.InsuranceZip.OrDbNull()
                };
                var InsuranceAgencyName = new SqlParameter
                {
                    ParameterName = "InsuranceAgencyName",
                    Value = insuranceInformation.AgencyCompanyName.OrDbNull()
                };
                var InsuranceAgencyAddress = new SqlParameter
                {
                    ParameterName = "InsuranceAgencyAddress",
                    Value = insuranceInformation.AgencyAddress.OrDbNull()
                };
                var InsuranceAgencyCity = new SqlParameter
                {
                    ParameterName = "InsuranceAgencyCity",
                    Value = insuranceInformation.AgencyCity.OrDbNull()
                };
                var InsuranceAgencyState = new SqlParameter
                {
                    ParameterName = "InsuranceAgencyState",
                    Value = insuranceInformation.AgencyState.OrDbNull()
                };
                var InsuranceAgencyZip = new SqlParameter
                {
                    ParameterName = "InsuranceAgencyZip",
                    Value = insuranceInformation.AgencyZip.OrDbNull()
                };
                var InsuredName = new SqlParameter
                {
                    ParameterName = "InsuredName",
                    Value = insuranceInformation.InsuredName.OrDbNull()
                };
                var InsuredAddress = new SqlParameter
                {
                    ParameterName = "InsuredAddress",
                    Value = insuranceInformation.InsuredAddress.OrDbNull()
                };
                var InsuredCity = new SqlParameter
                {
                    ParameterName = "InsuredCity",
                    Value = insuranceInformation.InsuredCity.OrDbNull()
                };
                var InsuredState = new SqlParameter
                {
                    ParameterName = "InsuredState",
                    Value = insuranceInformation.InsuredState.OrDbNull()
                };
                var InsuredZip = new SqlParameter
                {
                    ParameterName = "InsuredZip",
                    Value = insuranceInformation.InsuredZip.OrDbNull()
                };
                var InsuranceCardPath = new SqlParameter
                {
                    ParameterName = "InsuranceCardPath",
                    Value = path
                };
                var Filter = new SqlParameter
                {
                    ParameterName = "Filter",
                    Value = insuranceInformation.Filter.OrDbNull()
                };


                object[] parameters = new object[] { AcctNo, InsPolicyNo, InsEffDate, InsExpDate, InsuranceCompanyName, InsuranceCompanyAddress, InsuranceCompanyCity, InsuranceCompanyState, InsuranceCompanyZip, InsuranceAgencyName, InsuranceAgencyAddress, InsuranceAgencyCity, InsuranceAgencyState, InsuranceAgencyZip, InsuredName, InsuredAddress, InsuredCity, InsuredState, InsuredZip, InsuranceCardPath, Filter };

                var SaveInsuranceInformation = dataCollection.SaveInsuranceInformation(parameters);


                if (SaveInsuranceInformation == "complete")
                {
                    message = "Insurance Information is saved successfully.";
                   
 
                }
                else
                {
                    message = "Insurance Information is failed.";
                }

            }
            catch (Exception ex)
            {
                throw;
            }



            return message;
        }
        #endregion


        #region Get Insurance Information
        /// <summary>
        /////Get Insurance Information  function
        /// </summary>
        /// <param name="AccountNumber">AccountNumber</param>
        /// <returns>all Get Insurance Information</returns>
        public InsuranceInformationEntity GetInsuranceInformation(string accountNumber)
        {

            try
            {
                var AcctNo = new SqlParameter
                {
                    ParameterName = "AcctNo",
                    Value = accountNumber.OrDbNull()
                };
                
                object[] parameters = new object[] { AcctNo };

                var GetinsuranceInformation = dataCollection.GetInsuranceInformationDetail<InsuranceInformationEntity>(parameters).FirstOrDefault();

                return GetinsuranceInformation;

            }
            catch (Exception ex)
            {
                throw;
            }

           
        }
        #endregion

        
        #region Get Insurance Company List
            /// <summary>
        /////Get Insurance Company List
        /// </summary>
        /// <returns>all Get Insurance Company List</returns>
        public List<InsuranceInformationEntity> GetInsuranceCompanyList()
        {

            try
            {

                return dataCollection.GetInsuranceCompanyList<InsuranceInformationEntity>().ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


    }
}
