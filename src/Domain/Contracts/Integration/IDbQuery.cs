using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts.Integration
{
    public interface IDbQuery
    {
        void Execute(IDbContext context);
    }
}