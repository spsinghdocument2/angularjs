using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public interface ITextNotificationRepository<T> where T : class 
    {
        List<T> SQLQuery<T>(string sql, params object[] parameters);
        string SpExecuteSql(string sql, params object[] parameters);
    }
}
