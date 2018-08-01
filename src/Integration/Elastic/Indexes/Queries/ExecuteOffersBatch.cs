using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Elastic.Indexes.Errors;
using Elastic.Indexes.Models;
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
            var toDelete = this.actions.Where(x=>x.Action == CRUDActionType.Delete).Select(x=>(ESOffer)x.Entity).ToList();
            var toCreate = this.actions.Where(x=>x.Action == CRUDActionType.Create).Select(x=>(ESOffer)x.Entity).ToList();
            var toUpdate = this.actions.Where(x=>x.Action == CRUDActionType.Update).Select(x=>(ESOffer)x.Entity).ToList();

            var client = context.GetClient();
            var result = client.Bulk(x => {
                if(toCreate.Any())                
                    x=x.IndexMany(toCreate,(a,b)=>{return a.Id(b.Id);});

                if(toUpdate.Any())
                    x=x.IndexMany(toUpdate,(a,b)=>{return a.Id(b.Id);});
                    
                if(toDelete.Any())
                    x=x.DeleteMany(toDelete,(a,b)=>{return a.Id(b.Id);});    
                    
                return x;
            });

            if(!result.IsValid)
            {
                this.AddErrors(new QueryExectutionFailedError(result.DebugInformation));
            }
        }
    }
}