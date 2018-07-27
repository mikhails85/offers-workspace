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
    public class AddSkill: Result<long>, IDbQuery
    {
        private readonly Skill skill;

        public AddSkill(Skill skill)
        {
            this.skill = skill;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Query<long>(@"INSERT INTO Skills (Name) VALUES (@Name);
                                                          SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);",skill);                                                     
                SetValue(result.FirstOrDefault());
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}