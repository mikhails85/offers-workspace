using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elastic.Indexes;

namespace Elastic
{
    public class ESStorage: ElasicStorage
    {
        public ESStorage(ElasticSettings settings) 
                : base(settings)
        {
        }

        protected override void OnInitEEntityResolvers()
        {
            this.Set(x=>new EmployeeIndex(x));
            this.Set(x=>new OfferIndex(x));
        }
    }
}