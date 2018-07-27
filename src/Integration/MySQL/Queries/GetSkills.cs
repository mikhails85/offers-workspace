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
    public class GetSkills: Result<IEnumerable<Skill>>, IDbQuery
    {
        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();        
                
                var skills = dbConnection.Query<Skill>(@"SELECT * FROM Skills ");  

                base.SetValue(skills);
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }        
    }
}