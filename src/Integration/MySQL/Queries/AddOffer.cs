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
    public class AddOffer: Result<long>, IDbQuery
    {
        private readonly Offer offer;

        public AddOffer(Offer offer)
        {
            this.offer = offer;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Query<long>(@"INSERT INTO Offers (Name, Description) VALUES (@Name, @Description);
                                                          SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);", offer);                                                     
                SetValue(result.FirstOrDefault());
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}