using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Elastic.Indexes.Errors;
using Elastic.Indexes.Models;
using Models;
using Results;

namespace Elastic.Indexes.Queries
{
    public class GetTotalEmployees : Result<long>, IESQuery<Employee>
    {
        public void Execute(IESIndex<Employee> context)
        {
            var client = context.GetClient();
            var response = client.Search<ESEmployee>(s => s
                .From(0)
                .Size(0)
                .Query(q => q
                    .MatchAll())                        
            );

            if (!response.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(response.DebugInformation));
                return;    
            }

            this.SetValue(response.Total);
        }
    }
}