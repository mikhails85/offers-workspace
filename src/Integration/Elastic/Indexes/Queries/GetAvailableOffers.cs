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
    public class GetAvailableOffers : Result<IEnumerable<Offer>>, IESQuery<Offer>
    {
        private readonly int page;
        private readonly int size;
        private readonly ESEmployee employee;

        public GetAvailableOffers(int page, int size, Employee employee)
        {
            this.page = page;
            this.size = size;
            this.employee = employee;
        }

        public void Execute(IESIndex<Offer> context)
        {
            var client = context.GetClient();
            var searchResponse = client.Search<ESOffer>(s => s
                .From(page)
                .Size(size)
                .Query(q => q
                    .Percolate(p => p
                        .Field(f => f.Query)
                        .Document(employee)
                    )
                )
            );

            if (!searchResponse.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(searchResponse.DebugInformation));
                return;    
            }
                
            var offers = new List<Offer>();   
            foreach(var doc in searchResponse.Documents)         
            {
                offers.Add(doc);
            }

            SetValue(offers);
        }
    }
}