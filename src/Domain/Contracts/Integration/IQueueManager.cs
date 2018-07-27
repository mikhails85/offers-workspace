using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts.Integration
{
    public interface IQueueManager
    {
        IQueueListener Listener(IQueueHandler handler);
        
        void SendMessage<TMessage> (string queue, TMessage message);
    }
}