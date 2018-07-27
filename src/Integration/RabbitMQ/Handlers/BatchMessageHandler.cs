using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Newtonsoft.Json;
using Results;

namespace RabbitMq.Handlers
{
    public class BatchMessageHandler<TMessage>:IQueueHandler
    {        
        public ushort PrefetchCount{get;}
        public string Queue{get;}
        
        private readonly Func<IEnumerable<TMessage>, VoidResult> batchHandler; 
        private List<MQMessage<TMessage>> messages;
        private IQueueListener listener;        
        
        private object thisLock = new object();
        
        public BatchMessageHandler(string queue, Func<IEnumerable<TMessage>, VoidResult> handler, ushort count)
        {
            this.Queue = queue;
            this.batchHandler = handler;
            this.PrefetchCount = count;
            this.messages = new List<MQMessage<TMessage>>();
        }
                
        public void OnStarting()
        {            
        }
        
        public void HandleMessage(ulong tag, string message)
        {
            lock(thisLock)
            {
                this.messages.Add(new MQMessage<TMessage>{
                   Tag = tag,
                   Message = ParsMessage(message)
                });    
            }
                        
            if(this.messages.Count()>=this.PrefetchCount)
            {
                FatchMessages();
            }
        }
        
        public void SetListener(IQueueListener listener)
        {
            this.listener = listener;
        }
        
        public void OnStoping()
        {
            FatchMessages();
        }
        
        public void FatchMessages()
        {
            var batch = new List<MQMessage<TMessage>>();
            
            lock (thisLock)
            {
                batch.AddRange(this.messages);
                this.messages.Clear();
            }           
            
            var action = this.batchHandler;            
            var result = action(batch.Select(x=>x.Message).ToList());   
            
            if(result.Success)
            {
                ConfirmBatch(batch.Select(x=>x.Tag).ToList()); 
            }
            else
            {
                RejectBatch(batch.Select(x=>x.Tag).ToList());        
            }
        }
        
        private void RejectBatch(IEnumerable<ulong> batch)        
        {
            foreach(var b in batch)
            {
                this.listener.RejectMessage(b);    
            }            
        }
        
        private void ConfirmBatch(IEnumerable<ulong> batch)        
        {
            foreach(var b in batch)
            {
                this.listener.ConfirmMessage(b);    
            }            
        }
        
        private TMessage ParsMessage(string message)
        {
            return JsonConvert.DeserializeObject<TMessage>(message);
        }
    }
}