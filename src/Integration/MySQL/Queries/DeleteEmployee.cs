using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Dapper;
using Results;
using Results.Errors;

namespace MySql.Queries
{
    public class DeleteEmployee: VoidResult, IDbQuery
    {
        private readonly long employeeId;

        public DeleteEmployee(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                dbConnection.Execute("DELETE FROM Employees WHERE Id=@Id;", new {Id = employeeId }); 
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}