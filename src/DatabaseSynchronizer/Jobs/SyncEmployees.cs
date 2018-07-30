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
    public class SyncEmployees
    {
        private readonly IESStorage storage;
        private readonly IQueueManager queue;

        public SyncEmployees(IESStorage storage, IQueueManager queue)
        {
            this.storage =storage;
            this.queue = queue;    
        }

        public void Run()
        {
            var employeesListener = new BatchMessageHandler<CRUDWrapper<Employee>>("employees",BatchHandler, 10);
            queue.Listener(employeesListener).Start();
            var employeesTimer = new Timer(
                (o) => {
                    employeesListener.FatchMessages();
                },null,  10*1000, 10*1000);  
        }
        
        private VoidResult BatchHandler(IEnumerable<CRUDWrapper<Employee>> batch) 
        {
            if(!batch.Any())
                return new VoidResult();
                
            return this.storage.Get<Employee>().Query(new ExecuteEmployeesBatch(batch));
        }
    }
}