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
    public class DeleteProject: VoidResult, IDbQuery
    {
        private readonly long projectId;

        public DeleteProject(long projectId)
        {
            this.projectId = projectId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                dbConnection.Execute("DELETE FROM Projects WHERE Id=@Id;", new {Id = projectId }); 
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}