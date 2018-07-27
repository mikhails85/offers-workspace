using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elastic.Indexes.Models;
using Models;

namespace Elastic.Indexes
{
    public class OfferIndex: ESIndex<Offer>
    {
        private const string Index ="offers";

        public OfferIndex(ElasicStorage storage) 
            : base(storage, Index)
        {
        }

        protected override void CreateIndex()
        {
             var createIndexResponse = this.Client.CreateIndex(Index, c => c
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(0)
                )
                .Mappings(m => m                    
                    .Map<ESOffer>(mm => mm
                        .AutoMap()
                        .Properties(p => p                            
                            .Percolator(pp => pp
                                .Name(n => n.Query)
                            )
                        ).AutoMap<ESEmployee>()
                    )
                )
            );

            if(!createIndexResponse.IsValid)
            {
                throw new Exception(createIndexResponse.DebugInformation);
            }
        }
    }
}