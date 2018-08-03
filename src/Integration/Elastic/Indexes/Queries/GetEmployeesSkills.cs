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
    public class GetEmployeesSkills : Result<Dictionary<string, long>>, IESQuery<Employee>
    {
        public void Execute(IESIndex<Employee> context)
        {
            var client = context.GetClient();
            var response = client.Search<ESEmployee>(s => s
                    .Aggregations(a => a.Terms("skill_names", t => t.Field(p => p.Skills))));

            if (!response.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(response.DebugInformation));
                return;
            }

            var skills = response.Aggregations.Terms("skill_names");
            var result = new Dictionary<string, long>();
            foreach(var item in skills.Buckets)
            {                
                result.Add(item.Key, item.DocCount.GetValueOrDefault());
            }

            this.SetValue(result);
        }
    }
}