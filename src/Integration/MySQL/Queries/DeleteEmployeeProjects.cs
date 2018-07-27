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
    public class DeleteEmployeeProjects: VoidResult, IDbQuery
    {
        private readonly long employeeId;

        public DeleteEmployeeProjects(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                dbConnection.Execute("DELETE FROM Projects WHERE EmployeeId=@Id;", new {Id = employeeId }); 
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}