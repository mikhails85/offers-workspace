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
    public class SearchOffers: Result<IEnumerable<Offer>>, IESQuery<Offer>
    {
        private readonly int page;

        private readonly int size;

        private readonly string search;

        public SearchOffers(int page, int size, string search)
        {
            this.page = page;
            this.size = size;
            this.search = search?? string.Empty;
        }
        public void Execute(IESIndex<Offer> context)
        {
            var client = context.GetClient();
            var searchResponse = client.Search<ESOffer>(s => s
                .From(page)
                .Size(size)                
                .Query(q => q
                    .MultiMatch(mp => mp
                        .Query(search)	
                        .Fields(f => f	
                            .Fields(f1 => f1.Name))))
                        );

            if (!searchResponse.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(searchResponse.DebugInformation));
                return;    
            }
                
            var employees = new List<Offer>();   
            foreach(var doc in searchResponse.Documents)         
            {
                employees.Add(doc);
            }

            SetValue(employees);
        }
    }
}