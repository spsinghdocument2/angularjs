using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object Id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(Object Id);

        List<T> SQLQuery<T>(string sql, params object[] parameters);
         string SpExecuteSql(string sql, params object[] parameters);
        void Save();
    }
}
