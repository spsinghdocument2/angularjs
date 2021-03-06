﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MAF.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ReservesEntities1 : DbContext
    {
        public ReservesEntities1()
            : base("name=ReservesEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblOnlineUser> tblOnlineUsers { get; set; }
    
        public virtual ObjectResult<sp_PayOnlineVerify_Result> sp_PayOnlineVerify(string acctNo, string lastName)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_PayOnlineVerify_Result>("sp_PayOnlineVerify", acctNoParameter, lastNameParameter);
        }
    
        public virtual ObjectResult<string> sp_PayOnlineABA(string aBA)
        {
            var aBAParameter = aBA != null ?
                new ObjectParameter("ABA", aBA) :
                new ObjectParameter("ABA", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_PayOnlineABA", aBAParameter);
        }
    
        public virtual ObjectResult<sp_PayOnlineABA1_Result> sp_PayOnlineABA1(string aBA)
        {
            var aBAParameter = aBA != null ?
                new ObjectParameter("ABA", aBA) :
                new ObjectParameter("ABA", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_PayOnlineABA1_Result>("sp_PayOnlineABA1", aBAParameter);
        }
    
        public virtual int sp_PayOnlinePayment(string confirmation, string acctNo, Nullable<decimal> tranPayment, Nullable<decimal> tranFee, string bankABA, string bankAcctNo, string bankName, string bankHolder, string bankAcctType, string updatedPhone, Nullable<System.DateTime> updatedPhoneDate, string recurringPayment, string saveAccountFuture, string bankAccountName, Nullable<System.DateTime> futurePaymentDate, string scheduleMethod)
        {
            var confirmationParameter = confirmation != null ?
                new ObjectParameter("Confirmation", confirmation) :
                new ObjectParameter("Confirmation", typeof(string));
    
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var tranPaymentParameter = tranPayment.HasValue ?
                new ObjectParameter("TranPayment", tranPayment) :
                new ObjectParameter("TranPayment", typeof(decimal));
    
            var tranFeeParameter = tranFee.HasValue ?
                new ObjectParameter("TranFee", tranFee) :
                new ObjectParameter("TranFee", typeof(decimal));
    
            var bankABAParameter = bankABA != null ?
                new ObjectParameter("BankABA", bankABA) :
                new ObjectParameter("BankABA", typeof(string));
    
            var bankAcctNoParameter = bankAcctNo != null ?
                new ObjectParameter("BankAcctNo", bankAcctNo) :
                new ObjectParameter("BankAcctNo", typeof(string));
    
            var bankNameParameter = bankName != null ?
                new ObjectParameter("BankName", bankName) :
                new ObjectParameter("BankName", typeof(string));
    
            var bankHolderParameter = bankHolder != null ?
                new ObjectParameter("BankHolder", bankHolder) :
                new ObjectParameter("BankHolder", typeof(string));
    
            var bankAcctTypeParameter = bankAcctType != null ?
                new ObjectParameter("BankAcctType", bankAcctType) :
                new ObjectParameter("BankAcctType", typeof(string));
    
            var updatedPhoneParameter = updatedPhone != null ?
                new ObjectParameter("UpdatedPhone", updatedPhone) :
                new ObjectParameter("UpdatedPhone", typeof(string));
    
            var updatedPhoneDateParameter = updatedPhoneDate.HasValue ?
                new ObjectParameter("UpdatedPhoneDate", updatedPhoneDate) :
                new ObjectParameter("UpdatedPhoneDate", typeof(System.DateTime));
    
            var recurringPaymentParameter = recurringPayment != null ?
                new ObjectParameter("RecurringPayment", recurringPayment) :
                new ObjectParameter("RecurringPayment", typeof(string));
    
            var saveAccountFutureParameter = saveAccountFuture != null ?
                new ObjectParameter("SaveAccountFuture", saveAccountFuture) :
                new ObjectParameter("SaveAccountFuture", typeof(string));
    
            var bankAccountNameParameter = bankAccountName != null ?
                new ObjectParameter("BankAccountName", bankAccountName) :
                new ObjectParameter("BankAccountName", typeof(string));
    
            var futurePaymentDateParameter = futurePaymentDate.HasValue ?
                new ObjectParameter("FuturePaymentDate", futurePaymentDate) :
                new ObjectParameter("FuturePaymentDate", typeof(System.DateTime));
    
            var scheduleMethodParameter = scheduleMethod != null ?
                new ObjectParameter("ScheduleMethod", scheduleMethod) :
                new ObjectParameter("ScheduleMethod", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_PayOnlinePayment", confirmationParameter, acctNoParameter, tranPaymentParameter, tranFeeParameter, bankABAParameter, bankAcctNoParameter, bankNameParameter, bankHolderParameter, bankAcctTypeParameter, updatedPhoneParameter, updatedPhoneDateParameter, recurringPaymentParameter, saveAccountFutureParameter, bankAccountNameParameter, futurePaymentDateParameter, scheduleMethodParameter);
        }
    
        public virtual ObjectResult<SP_GetUserProfile_Result> SP_GetUserProfile(string accountNo)
        {
            var accountNoParameter = accountNo != null ?
                new ObjectParameter("accountNo", accountNo) :
                new ObjectParameter("accountNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetUserProfile_Result>("SP_GetUserProfile", accountNoParameter);
        }
    
        public virtual int SP_UpdateUserProfile(string email, string cellNumber, string textNumber, string address, string accountNumber)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var cellNumberParameter = cellNumber != null ?
                new ObjectParameter("CellNumber", cellNumber) :
                new ObjectParameter("CellNumber", typeof(string));
    
            var textNumberParameter = textNumber != null ?
                new ObjectParameter("TextNumber", textNumber) :
                new ObjectParameter("TextNumber", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var accountNumberParameter = accountNumber != null ?
                new ObjectParameter("AccountNumber", accountNumber) :
                new ObjectParameter("AccountNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateUserProfile", emailParameter, cellNumberParameter, textNumberParameter, addressParameter, accountNumberParameter);
        }
    
        public virtual ObjectResult<sp_PayOnlineForgot_Result> sp_PayOnlineForgot(string acctNo)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_PayOnlineForgot_Result>("sp_PayOnlineForgot", acctNoParameter);
        }
    
        public virtual int sp_updateSecurityQuestions(string acctNo, Nullable<byte> securityID, string answer, Nullable<byte> securityID2, string answer2, Nullable<byte> securityID3, string answer3)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var securityIDParameter = securityID.HasValue ?
                new ObjectParameter("SecurityID", securityID) :
                new ObjectParameter("SecurityID", typeof(byte));
    
            var answerParameter = answer != null ?
                new ObjectParameter("Answer", answer) :
                new ObjectParameter("Answer", typeof(string));
    
            var securityID2Parameter = securityID2.HasValue ?
                new ObjectParameter("SecurityID2", securityID2) :
                new ObjectParameter("SecurityID2", typeof(byte));
    
            var answer2Parameter = answer2 != null ?
                new ObjectParameter("Answer2", answer2) :
                new ObjectParameter("Answer2", typeof(string));
    
            var securityID3Parameter = securityID3.HasValue ?
                new ObjectParameter("SecurityID3", securityID3) :
                new ObjectParameter("SecurityID3", typeof(byte));
    
            var answer3Parameter = answer3 != null ?
                new ObjectParameter("Answer3", answer3) :
                new ObjectParameter("Answer3", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_updateSecurityQuestions", acctNoParameter, securityIDParameter, answerParameter, securityID2Parameter, answer2Parameter, securityID3Parameter, answer3Parameter);
        }
    
        public virtual ObjectResult<sp_PayOnlineLogin_Result> sp_PayOnlineLogin(string acctNo, string username, string password)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_PayOnlineLogin_Result>("sp_PayOnlineLogin", acctNoParameter, usernameParameter, passwordParameter);
        }
    
        public virtual int sp_PayOnlineChange(string newOne, string acctNo, string bitReset)
        {
            var newOneParameter = newOne != null ?
                new ObjectParameter("NewOne", newOne) :
                new ObjectParameter("NewOne", typeof(string));
    
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var bitResetParameter = bitReset != null ?
                new ObjectParameter("BitReset", bitReset) :
                new ObjectParameter("BitReset", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_PayOnlineChange", newOneParameter, acctNoParameter, bitResetParameter);
        }
    
        public virtual int sp_PayOnlineRegister(string acctNo, string fullName, string password, string email, Nullable<byte> securityID, string answer, Nullable<byte> securityID2, string answer2, Nullable<byte> securityID3, string answer3, string bitReset)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("FullName", fullName) :
                new ObjectParameter("FullName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var securityIDParameter = securityID.HasValue ?
                new ObjectParameter("SecurityID", securityID) :
                new ObjectParameter("SecurityID", typeof(byte));
    
            var answerParameter = answer != null ?
                new ObjectParameter("Answer", answer) :
                new ObjectParameter("Answer", typeof(string));
    
            var securityID2Parameter = securityID2.HasValue ?
                new ObjectParameter("SecurityID2", securityID2) :
                new ObjectParameter("SecurityID2", typeof(byte));
    
            var answer2Parameter = answer2 != null ?
                new ObjectParameter("Answer2", answer2) :
                new ObjectParameter("Answer2", typeof(string));
    
            var securityID3Parameter = securityID3.HasValue ?
                new ObjectParameter("SecurityID3", securityID3) :
                new ObjectParameter("SecurityID3", typeof(byte));
    
            var answer3Parameter = answer3 != null ?
                new ObjectParameter("Answer3", answer3) :
                new ObjectParameter("Answer3", typeof(string));
    
            var bitResetParameter = bitReset != null ?
                new ObjectParameter("BitReset", bitReset) :
                new ObjectParameter("BitReset", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_PayOnlineRegister", acctNoParameter, fullNameParameter, passwordParameter, emailParameter, securityIDParameter, answerParameter, securityID2Parameter, answer2Parameter, securityID3Parameter, answer3Parameter, bitResetParameter);
        }
    
        public virtual ObjectResult<USP_GetPaymentSchedule_Result> USP_GetPaymentSchedule(string acctno)
        {
            var acctnoParameter = acctno != null ?
                new ObjectParameter("Acctno", acctno) :
                new ObjectParameter("Acctno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetPaymentSchedule_Result>("USP_GetPaymentSchedule", acctnoParameter);
        }
    
        public virtual ObjectResult<SP_GetUserProfile_1_Result> SP_GetUserProfile_1(string accountNo)
        {
            var accountNoParameter = accountNo != null ?
                new ObjectParameter("accountNo", accountNo) :
                new ObjectParameter("accountNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetUserProfile_1_Result>("SP_GetUserProfile_1", accountNoParameter);
        }
    
        public virtual int SP_UpdateUserProfile_1(string email, string cellNumber, string textNumber, string textNumber2, string address, string accountNumber, string profilePicture)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var cellNumberParameter = cellNumber != null ?
                new ObjectParameter("CellNumber", cellNumber) :
                new ObjectParameter("CellNumber", typeof(string));
    
            var textNumberParameter = textNumber != null ?
                new ObjectParameter("TextNumber", textNumber) :
                new ObjectParameter("TextNumber", typeof(string));
    
            var textNumber2Parameter = textNumber2 != null ?
                new ObjectParameter("TextNumber2", textNumber2) :
                new ObjectParameter("TextNumber2", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var accountNumberParameter = accountNumber != null ?
                new ObjectParameter("AccountNumber", accountNumber) :
                new ObjectParameter("AccountNumber", typeof(string));
    
            var profilePictureParameter = profilePicture != null ?
                new ObjectParameter("ProfilePicture", profilePicture) :
                new ObjectParameter("ProfilePicture", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateUserProfile_1", emailParameter, cellNumberParameter, textNumberParameter, textNumber2Parameter, addressParameter, accountNumberParameter, profilePictureParameter);
        }
    
        public virtual ObjectResult<sp_CheckDuplicatePayment_Result> sp_CheckDuplicatePayment(string acctNo)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_CheckDuplicatePayment_Result>("sp_CheckDuplicatePayment", acctNoParameter);
        }
    
        public virtual ObjectResult<SP_GetAchDetail_Result> SP_GetAchDetail(string acctNo)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetAchDetail_Result>("SP_GetAchDetail", acctNoParameter);
        }
    
        public virtual int sp_DeleteSavedAchDetail(string acctNo, string iD)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var iDParameter = iD != null ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_DeleteSavedAchDetail", acctNoParameter, iDParameter);
        }
    
        public virtual ObjectResult<sp_InsuranceInformation_Result> sp_InsuranceInformation(string acctNo, string insuranceCompanyName, string insuranceAddress, string insuranceCity, string insuranceState, string insuranceZip, string agencyCompanyName, string agencyAddress, string agencyCity, string agencyState, string agencyZip, string insuredName, string insuredAddress, string insuredCity, string insuredState, string insuredZip, string documentPath, string insPolicyNo, Nullable<System.DateTime> insEffDate, Nullable<System.DateTime> insExpDate, string filter)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            var insuranceCompanyNameParameter = insuranceCompanyName != null ?
                new ObjectParameter("InsuranceCompanyName", insuranceCompanyName) :
                new ObjectParameter("InsuranceCompanyName", typeof(string));
    
            var insuranceAddressParameter = insuranceAddress != null ?
                new ObjectParameter("InsuranceAddress", insuranceAddress) :
                new ObjectParameter("InsuranceAddress", typeof(string));
    
            var insuranceCityParameter = insuranceCity != null ?
                new ObjectParameter("InsuranceCity", insuranceCity) :
                new ObjectParameter("InsuranceCity", typeof(string));
    
            var insuranceStateParameter = insuranceState != null ?
                new ObjectParameter("InsuranceState", insuranceState) :
                new ObjectParameter("InsuranceState", typeof(string));
    
            var insuranceZipParameter = insuranceZip != null ?
                new ObjectParameter("InsuranceZip", insuranceZip) :
                new ObjectParameter("InsuranceZip", typeof(string));
    
            var agencyCompanyNameParameter = agencyCompanyName != null ?
                new ObjectParameter("AgencyCompanyName", agencyCompanyName) :
                new ObjectParameter("AgencyCompanyName", typeof(string));
    
            var agencyAddressParameter = agencyAddress != null ?
                new ObjectParameter("AgencyAddress", agencyAddress) :
                new ObjectParameter("AgencyAddress", typeof(string));
    
            var agencyCityParameter = agencyCity != null ?
                new ObjectParameter("AgencyCity", agencyCity) :
                new ObjectParameter("AgencyCity", typeof(string));
    
            var agencyStateParameter = agencyState != null ?
                new ObjectParameter("AgencyState", agencyState) :
                new ObjectParameter("AgencyState", typeof(string));
    
            var agencyZipParameter = agencyZip != null ?
                new ObjectParameter("AgencyZip", agencyZip) :
                new ObjectParameter("AgencyZip", typeof(string));
    
            var insuredNameParameter = insuredName != null ?
                new ObjectParameter("InsuredName", insuredName) :
                new ObjectParameter("InsuredName", typeof(string));
    
            var insuredAddressParameter = insuredAddress != null ?
                new ObjectParameter("InsuredAddress", insuredAddress) :
                new ObjectParameter("InsuredAddress", typeof(string));
    
            var insuredCityParameter = insuredCity != null ?
                new ObjectParameter("InsuredCity", insuredCity) :
                new ObjectParameter("InsuredCity", typeof(string));
    
            var insuredStateParameter = insuredState != null ?
                new ObjectParameter("InsuredState", insuredState) :
                new ObjectParameter("InsuredState", typeof(string));
    
            var insuredZipParameter = insuredZip != null ?
                new ObjectParameter("InsuredZip", insuredZip) :
                new ObjectParameter("InsuredZip", typeof(string));
    
            var documentPathParameter = documentPath != null ?
                new ObjectParameter("DocumentPath", documentPath) :
                new ObjectParameter("DocumentPath", typeof(string));
    
            var insPolicyNoParameter = insPolicyNo != null ?
                new ObjectParameter("InsPolicyNo", insPolicyNo) :
                new ObjectParameter("InsPolicyNo", typeof(string));
    
            var insEffDateParameter = insEffDate.HasValue ?
                new ObjectParameter("InsEffDate", insEffDate) :
                new ObjectParameter("InsEffDate", typeof(System.DateTime));
    
            var insExpDateParameter = insExpDate.HasValue ?
                new ObjectParameter("InsExpDate", insExpDate) :
                new ObjectParameter("InsExpDate", typeof(System.DateTime));
    
            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_InsuranceInformation_Result>("sp_InsuranceInformation", acctNoParameter, insuranceCompanyNameParameter, insuranceAddressParameter, insuranceCityParameter, insuranceStateParameter, insuranceZipParameter, agencyCompanyNameParameter, agencyAddressParameter, agencyCityParameter, agencyStateParameter, agencyZipParameter, insuredNameParameter, insuredAddressParameter, insuredCityParameter, insuredStateParameter, insuredZipParameter, documentPathParameter, insPolicyNoParameter, insEffDateParameter, insExpDateParameter, filterParameter);
        }
    
        public virtual ObjectResult<sp_GetInsuranceInformation_Result> sp_GetInsuranceInformation(string acctNo)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetInsuranceInformation_Result>("sp_GetInsuranceInformation", acctNoParameter);
        }
    
        public virtual ObjectResult<sp_GetAlerts_Result> sp_GetAlerts(string accountNumber)
        {
            var accountNumberParameter = accountNumber != null ?
                new ObjectParameter("accountNumber", accountNumber) :
                new ObjectParameter("accountNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAlerts_Result>("sp_GetAlerts", accountNumberParameter);
        }
    
        public virtual ObjectResult<sp_GetPayByTextSetup_Result> sp_GetPayByTextSetup(string acctNo)
        {
            var acctNoParameter = acctNo != null ?
                new ObjectParameter("AcctNo", acctNo) :
                new ObjectParameter("AcctNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetPayByTextSetup_Result>("sp_GetPayByTextSetup", acctNoParameter);
        }
    }
}
