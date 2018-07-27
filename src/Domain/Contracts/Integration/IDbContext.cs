using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts.Integration
{
    public interface IDbContext: IDisposable
    {
        TQuery Query<TQuery>(TQuery query) where TQuery:IDbQuery;
        void Save();
    }
}