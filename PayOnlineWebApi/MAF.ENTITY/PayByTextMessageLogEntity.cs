
namespace MAF.ENTITY
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PayByTextMessageLogEntity
    {
        public string AcctNo { get; set; }
        public string TextNumber { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Keyword { get; set; }
        public DateTime Date { get; set; }
    }
}
