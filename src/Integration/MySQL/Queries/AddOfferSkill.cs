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
    public class AddOfferSkill: Result<long>, IDbQuery
    {
        private readonly long skillId;
        private readonly long offerId;

        public AddOfferSkill(long offerId, long skillId)
        {
            this.offerId = offerId;
            this.skillId = skillId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Query<long>(@"INSERT INTO OffersSkills (OfferId, SkillId) VALUES (@OfferId, @SkillId);
                                                          SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);", new{OfferId=offerId, SkillId = skillId});                                                     
                SetValue(result.FirstOrDefault());
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}