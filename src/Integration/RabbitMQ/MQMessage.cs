using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RabbitMq
{
    public class MQMessage<TMessage>
    {
        public ulong Tag {get;set;}
        public TMessage Message {get;set;}
    }
}