using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Contracts.Integration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq
{
    public class MQListener : IQueueListener
    {
        private readonly MQSettings settings;
        private readonly IQueueHandler handler;
        
        private IConnection connection;        
        private IModel channel;
                
        public IQueueManager QueueManager {get;set;}
                
        public MQListener(MQSettings settings, IQueueHandler handler)
        {
            this.settings = settings;
            this.handler = handler;            
            this.handler.SetListener(this);            
        }
        
        public void SetManager(IQueueManager manager)
        {
            QueueManager = manager;
        }
        
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = settings.Host };
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();   
            
            this.channel.QueueDeclare(queue: this.handler.Queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            
            this.channel.BasicQos(prefetchSize: 0, prefetchCount: this.handler.PrefetchCount, global: false);

            this.handler.OnStarting();                        
            
            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += (s,e)=>{                
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);            
                this.handler.HandleMessage(e.DeliveryTag, message);
            };     
            
            this.channel.BasicConsume(this.handler.Queue, false, consumer);
        }
        
        public void RejectMessage(ulong tag)
        {
            this.channel.BasicReject(tag, true);
        }
        
        public void ConfirmMessage(ulong tag)
        {
            this.channel.BasicAck(tag, false);
        }
        
        public void Stop()
        {
            this.handler.OnStoping();
            
            this.channel.Close(200, "Goodbye");
            this.connection.Close();
        }
    }
}