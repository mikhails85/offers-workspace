using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Elastic.Indexes.Errors;
using Elastic.Indexes.Models;
using Models;
using Nest;
using Results;

namespace Elastic.Indexes.Queries
{
    public class GetOffersSkills : Result<Dictionary<string, long>>, IESQuery<Offer>
    {
        public void Execute(IESIndex<Offer> context)
        {
            var client = context.GetClient();
            var response = client.Search<ESOffer>(s => s
                    .Aggregations(a => a.Terms("skills", t => t.Field(p => p.RequaredSkills.First().Id))));
            if (!response.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(response.DebugInformation));
                return;
            }

            var skills = response.Aggregations.Terms("skills");
            var result = new Dictionary<string, long>();
            foreach(var item in skills.Buckets)
            {                
                result.Add(item.Key, item.DocCount.GetValueOrDefault());
            }

            this.SetValue(result);
        }
    }
}