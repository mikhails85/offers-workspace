using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Dapper;
using Models;
using Results;
using Results.Errors;

namespace MySql.Queries
{
    public class AddEmployee: Result<long>, IDbQuery
    {
        private readonly Employee employee;

        public AddEmployee(Employee employee)
        {
            this.employee = employee;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Query<long>(@"INSERT INTO Employees (Name, JobTitle, Photo, CV) VALUES (@Name, @JobTitle, @Photo, @CV);
                                                          SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);", employee);                                                     
                SetValue(result.FirstOrDefault());
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}