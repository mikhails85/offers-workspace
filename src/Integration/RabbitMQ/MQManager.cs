using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Contracts.Integration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMq
{
    public class MQManager:IQueueManager
    {
        private readonly MQSettings settings;
        
        public MQManager(MQSettings settings)
        {
            this.settings = settings;    
        }
        
        public void SendMessage<TMessage> (string queue, TMessage message)
        {
            var factory = new ConnectionFactory() { HostName = this.settings.Host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                var body = Encoding.UTF8.GetBytes(SerializeMessage(message));

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);                
            }        
        }
        
        public IQueueListener Listener(IQueueHandler handler)
        {
            var listener = new MQListener(this.settings, handler);
            listener.SetManager(this);
            return listener;
        }
        
        private string SerializeMessage<TMessage>(TMessage message) 
        {
            return JsonConvert.SerializeObject(message);
        }
    }
}