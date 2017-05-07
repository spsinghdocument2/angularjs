using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MAF.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private ReservesEntities1 db;
        private DbSet<T> dbSet;

        public Repository()
        {
            try
            {
                db = new ReservesEntities1();
                dbSet = db.Set<T>();
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                return dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public T GetById(object Id)
        {
            try
            {
                return dbSet.Find(Id);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public void Insert(T dbEntity)
        {
            try
            {
                dbSet.Add(dbEntity);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public void Update(T dbEntity)
        {
            try
            {
                db.Entry(dbEntity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public void Delete(object Id)
        {
            try
            {
                T getObjById = dbSet.Find(Id);
                dbSet.Remove(getObjById);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public void Save()
        {
            try
            {
                db.SaveChanges();
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
             
              
             var ExecuteSqlCommand =  db.Database.ExecuteSqlCommand(sql, parameters);
             
             message =   "complete";
            
            }
            catch
            {
                throw;
              //message ="Error";
            }
            
            return message;
        }

    }  
}
