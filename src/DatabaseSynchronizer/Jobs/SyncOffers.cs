using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Contracts.Integration;
using Elastic.Indexes.Queries;
using Models;
using RabbitMq.Handlers;
using Results;
using Wrappers;

namespace DatabaseSynchronizer.Jobs
{
    public class SyncOffers
    {
        private readonly IESStorage storage;
        private readonly IQueueManager queue;

        public SyncOffers(IESStorage storage, IQueueManager queue)
        {
            this.storage =storage;
            this.queue = queue;    
        }

        public void Run()
        {
            var offersListener = new BatchMessageHandler<CRUDWrapper<Offer>>("offers",BatchHandler, 10);
            this.queue.Listener(offersListener).Start();
            var offersTimer = new Timer(
                (o) => {
                    offersListener.FatchMessages();
                },null,  10*1000, 10*1000);
        }
        private VoidResult BatchHandler(IEnumerable<CRUDWrapper<Offer>> batch) 
        {
             if(!batch.Any())
                return new VoidResult();
                
            return this.storage.Get<Offer>().Query(new ExecuteOffersBatch(batch));
        }
    }
}