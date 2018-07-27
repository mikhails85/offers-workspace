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
    public class AddProject: Result<long>, IDbQuery
    {
        private readonly Project project;

        public AddProject(Project project)
        {
            this.project = project;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Query<long>(@"INSERT INTO Projects (EmployeeId, Name, Description) VALUES (@EmployeeId, @Name, @Description);
                                                          SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);", project);                                                     
                SetValue(result.FirstOrDefault());
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}