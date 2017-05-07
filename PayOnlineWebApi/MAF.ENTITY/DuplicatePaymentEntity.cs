using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
     public class DuplicatePaymentEntity
     {
         public string OneTime { get; set; }
         public string Scheduled { get; set; }   
         public string AutoPayDueDate { get; set; }
         public string FuturePayDate { get; set; }  
    }
}
