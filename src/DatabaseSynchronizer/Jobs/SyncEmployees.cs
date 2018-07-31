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
    public class SyncEmployees
    {
        private readonly IESStorage storage;
        private readonly IQueueManager queue;
        private readonly ILogger logger;

        public SyncEmployees(IESStorage storage, IQueueManager queue, ILogger logger)
        {
            this.storage =storage;
            this.queue = queue;    
            this.logger = logger;
        }

        public void Run()
        {
            var employeesListener = new BatchMessageHandler<CRUDWrapper<Employee>>("employees",BatchHandler, 10);
            queue.Listener(employeesListener).Start();
            var employeesTimer = new Timer(
                (o) => {
                    this.logger.LogInfo("Employee batch 'Fatch Messages'");  
                    employeesListener.FatchMessages();
                },null,  10*1000, 10*1000);  
        }

        private VoidResult BatchHandler(IEnumerable<CRUDWrapper<Employee>> batch) 
        {
            if(!batch.Any())
                return new VoidResult();
            this.logger.LogInfo($"Employee batch count:{batch.Count()}");       

            var result = this.storage.Get<Employee>().Query(new ExecuteEmployeesBatch(batch));            
            if(!result.Success)
            {
                this.logger.LogError($"Employee batch result:{result.Errors[0].Message}");       
            }
            return result;
        }
    }
}