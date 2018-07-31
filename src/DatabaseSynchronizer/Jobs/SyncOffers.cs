using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Contracts.Integration;
using Elastic.Indexes.Queries;
using Logger;
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
        private readonly ILogger logger;

        public SyncOffers(IESStorage storage, IQueueManager queue, ILogger logger)
        {
            this.storage =storage;
            this.queue = queue; 
            this.logger = logger;   
        }

        public void Run()
        {
            var offersListener = new BatchMessageHandler<CRUDWrapper<Offer>>("offers",BatchHandler, 10);
            this.queue.Listener(offersListener).Start();
            var offersTimer = new Timer(
                (o) => {
                    this.logger.LogInfo("Offer batch 'Fatch Messages'");  
                    offersListener.FatchMessages();
                },null,  10*1000, 10*1000);
        }
        private VoidResult BatchHandler(IEnumerable<CRUDWrapper<Offer>> batch) 
        {            
             if(!batch.Any())
                return new VoidResult();
            this.logger.LogInfo($"Offer batch count:{batch.Count()}");       
            var result = this.storage.Get<Offer>().Query(new ExecuteOffersBatch(batch));
            if(!result.Success)
            {
                this.logger.LogError($"Offer batch result:{result.Errors[0]}");       
            }
            return result;
        }
    }
}