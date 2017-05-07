using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class InsuranceInformationEntity
    {
        public string AccountNumber { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string InsuranceAddress { get; set; }
        public string InsuranceCity { get; set; }
        public string InsuranceState { get; set; }
        public string InsuranceZip { get; set; }

        public string AgencyCompanyName { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyState { get; set; }
        public string AgencyZip { get; set; }

        public string InsuredName { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredCity { get; set; }
        public string InsuredState { get; set; }
        public string InsuredZip { get; set; }

        public string PolicyNumber { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VehicleIdentificationNumber { get; set; }
        public string InsuranceCardPath { get; set; }


        public string AcctNo { get; set; }
        public string InsPolicyNo { get; set; }
        public Nullable<System.DateTime> InsEffDate { get; set; }
        public Nullable<System.DateTime> InsExpDate { get; set; }
        public string VehVin { get; set; }
        public Nullable<int> VehYear { get; set; }
        public string VehModel { get; set; }
        public string VehType { get; set; }
       
        public string InsuranceCompanyAddress { get; set; }
        public string InsuranceCompanyCity { get; set; }
        public string InsuranceCompanyState { get; set; }
        public string InsuranceCompanyZip { get; set; }
        public string InsuranceAgencyName { get; set; }
        public string InsuranceAgencyAddress { get; set; }
        public string InsuranceAgencyCity { get; set; }
        public string InsuranceAgencyState { get; set; }
        public string InsuranceAgencyZip { get; set; }
        public string Filter { get; set; }


    }
}
