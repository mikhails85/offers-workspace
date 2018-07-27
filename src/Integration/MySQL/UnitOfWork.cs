using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Contracts.Integration;
using MySql.Data.MySqlClient;

namespace MySql
{
    public class UnitOfWork:IDbContext
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        
        private bool disposed;
        
        public UnitOfWork (DbSettings settings)
        {
            this.connection = new MySqlConnection(settings.ConnectionString);
            this.connection.Open();
            this.transaction = this.connection.BeginTransaction();
        }
        
        internal IDbTransaction Transaction => this.transaction;
        
        public TQuery Query<TQuery>(TQuery query) where TQuery:IDbQuery
        {
            query.Execute(this);
            return query;
        }
       
        public void Save()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                transaction = connection.BeginTransaction();
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if(disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if(connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
                disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}