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
    public class SearchEmployees: Result<IEnumerable<Employee>>, IESQuery<Employee>
    {
        private readonly int page;

        public int size { get; }

        private readonly string search;

        public SearchEmployees(int page, int size, string search)
        {
            this.page = page;
            this.size = size;
            this.search = search;
        }
        public void Execute(IESIndex<Employee> context)
        {
            var client = context.GetClient();
            var searchResponse = client.Search<ESEmployee>(s => s
                .From(page)
                .Size(size)
                .Query(q => q
                    .MultiMatch(mp => mp
                        .Query(search)	
                        .Fields(f => f	
                            .Fields(f1 => f1.Name, f2 => f2.JobTitle))))
                        
            );

            if (!searchResponse.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(searchResponse.DebugInformation));
                return;    
            }
                
            var employees = new List<Employee>();   
            foreach(var doc in searchResponse.Documents)         
            {
                employees.Add(doc);
            }

            SetValue(employees);
        }
    }
}