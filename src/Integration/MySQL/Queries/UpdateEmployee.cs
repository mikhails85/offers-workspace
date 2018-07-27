using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Dapper;
using Models;
using MySql.Errors;
using Results;
using Results.Errors;

namespace MySql.Queries
{
    public class UpdateEmployee: VoidResult, IDbQuery
    {
        private readonly Employee employee;

        public UpdateEmployee(Employee employee)
        {
            this.employee = employee;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Execute("UPDATE Employees SET Name = @Name, JobTitle= @JobTitle, Photo = @Photo, CV = @CV WHERE Id = @Id;", employee);                                                     
                if(result <1)
                {
                    base.AddErrors(new SQLExecutionFailedError("UPDATE Employees"));    
                }
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}