using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wrappers
{
    public class CRUDWrapper<TEntity>
    {
        public TEntity Entity { get; set; }
        public CRUDActionType Action { get; set; }
    }
}