//------------------------------------------------------------------------------
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
    
    public partial class sp_PayOnlineVerify_Result
    {
        public string AcctNo { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> AcctCurrentBal { get; set; }
        public Nullable<decimal> AcctRegPayAmt { get; set; }
        public Nullable<decimal> AcctPastDueAmt { get; set; }
        public Nullable<int> VehYear { get; set; }
        public string VehModel { get; set; }
        public string VehType { get; set; }
        public string State { get; set; }
        public string DlrAcctNo { get; set; }
        public string UpdatedPhone { get; set; }
        public Nullable<System.DateTime> UpdatedPhoneDate { get; set; }
    }
}
