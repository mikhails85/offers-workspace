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
    public class GetAvailableEmployees : Result<IEnumerable<Employee>>, IESQuery<Employee>
    {
        private readonly int page;
        private readonly int size;
        private readonly Offer offer;

        public GetAvailableEmployees(int page, int size, Offer offer)
        {
            this.page = page;
            this.size = size;
            this.offer = offer;
        }
        public void Execute(IESIndex<Employee> context)
        {
            var client = context.GetClient();
            var query = new QueryContainer();
            foreach(var skill in offer.RequaredSkills)    
            {
                query &= new MatchQuery{
                    Field = new Field("skills.id"),
                    Query = skill.Id.ToString()    
                };
            }
            var searchResponse = client.Search<ESEmployee>(s => s
                .From(page)
                .Size(size)
                .Query(q=> query)                        
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