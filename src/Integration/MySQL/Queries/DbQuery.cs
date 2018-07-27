using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Contracts.Integration;

namespace MySql.Queries
{
    public static class DbQuery
    {
        public static IDbTransaction GetTransaction(this IDbContext context)
        {           
            if(!(context is UnitOfWork))
            {
                throw new NotSupportedException("Context should be 'UnitOfWork' type"); 
            }
            
            return ((UnitOfWork)context).Transaction;
        }
        
        public static IDbConnection GetConnection(this IDbContext context)
        {
            return context.GetTransaction().Connection;
        }
    }
}