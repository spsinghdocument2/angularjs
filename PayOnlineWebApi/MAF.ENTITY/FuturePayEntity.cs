using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class FuturePayEntity
    {
        public Nullable<decimal> Amount { get; set; }
        public string DueDate { get; set; }
        public string NextDueDate { get; set; }
        public string Name { get; set; }
        public string CellNumber { get; set; }
        public Nullable<int> AcctDaysPastDue { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
    }
}
