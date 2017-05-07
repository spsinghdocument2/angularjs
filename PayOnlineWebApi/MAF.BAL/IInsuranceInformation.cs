using MAF.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public  interface IInsuranceInformation
    {
        string SaveInsuranceInformation(InsuranceInformationEntity insuranceInformation);
        InsuranceInformationEntity GetInsuranceInformation(string accountNumber);
        List<InsuranceInformationEntity> GetInsuranceCompanyList();
    }
}
