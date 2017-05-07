using MAF.ENTITY;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.BAL
{
    public class Global
    {
        private static readonly Global instance = new Global();
        internal ConcurrentDictionary<int, List<PaymentHistoryEntity>> holder = new ConcurrentDictionary<int, List<PaymentHistoryEntity>>();
        internal ConcurrentDictionary<int, List<SearchPaymentDetailsEntity>> searchPaymentHolder = new ConcurrentDictionary<int, List<SearchPaymentDetailsEntity>>();
      
        public static Global Instance
        {
            get
            {
                return instance;
            }
        }
      
    }

    public static class SqlParameterHelper
    {
        public static object OrDbNull(this string s)
        {
            return string.IsNullOrEmpty(s) ? DBNull.Value : (object)s;
        }
    }
}
