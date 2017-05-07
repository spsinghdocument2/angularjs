using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class SearchPaymentDetailsEntity
    {
        public string AccountNumber { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string ScheduleMethod { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }        
        public string ScheduledAmount { get; set; }
        public string PaymentFrequency {get;set;}   
        public string PaymentMode { get; set; }
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isactive { get; set; }
    }
}
