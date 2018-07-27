using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts.Integration
{
    public interface IQueueHandler
    {
        ushort PrefetchCount{get;}
        string Queue{get;}
        
        void OnStarting();     
        void HandleMessage(ulong tag, string message);
        void SetListener(IQueueListener listener);
        void OnStoping();
    }
}