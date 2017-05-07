using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public class TextNotificationRepository<T>:ITextNotificationRepository<T> where T : class
    {
        private TextNotificationEntities db;

        public TextNotificationRepository()
        {
            try
            {
                db = new TextNotificationEntities();
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        public List<T> SQLQuery<T>(string sql, params object[] parameters)
        {
            try
            {
                return db.Database.SqlQuery<T>(sql, parameters).ToList<T>();
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public string SpExecuteSql(string sql, params object[] parameters)
        {
            string message = string.Empty;
            try
            {


                var ExecuteSqlCommand = db.Database.ExecuteSqlCommand(sql, parameters);
                message = "complete";

            }
            catch 
            {
                throw;
            }

            return message;
        }
    }
}
