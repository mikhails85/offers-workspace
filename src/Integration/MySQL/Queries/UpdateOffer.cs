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
    public class UpdateOffer: VoidResult, IDbQuery
    {
        private readonly Offer offer;

        public UpdateOffer(Offer offer)
        {
            this.offer = offer;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();            
                var result = dbConnection.Execute("UPDATE Offers SET Name = @Name, Description= @Description WHERE Id = @Id;", offer);                                                     
                if(result <1)
                {
                    base.AddErrors(new SQLExecutionFailedError("UPDATE Offers"));    
                }
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }
    }
}