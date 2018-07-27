using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Models;
using Results;
using Wrappers;

namespace Elastic.Indexes.Queries
{
    public class ExecuteOffersBatch:VoidResult, IESQuery<Offer>
    {
        private readonly IEnumerable<CRUDWrapper<Offer>> actions;

        public ExecuteOffersBatch(IEnumerable<CRUDWrapper<Offer>> actions)
        {
            this.actions = actions;
        }

        public void Execute(IESIndex<Offer> context)
        {
            var toDelete = this.actions.Where(x=>x.Action == CRUDActionType.Delete).Select(x=>x.Entity).ToList();
            var toCreate = this.actions.Where(x=>x.Action == CRUDActionType.Create).Select(x=>x.Entity).ToList();
            var toUpdate = this.actions.Where(x=>x.Action == CRUDActionType.Update).Select(x=>x.Entity).ToList();

            var client = context.GetClient();
            client.Bulk(x => {
                if(toCreate.Any())                
                    x=x.IndexMany(toCreate,(a,b)=>{return a.Id(b.Id);});

                if(toUpdate.Any())
                    x=x.UpdateMany(toUpdate,(a,b)=>{return a.Id(b.Id);});
                    
                 if(toDelete.Any())
                    x=x.DeleteMany(toDelete,(a,b)=>{return a.Id(b.Id);});    
                    
                return x;
            });
        }
    }
}