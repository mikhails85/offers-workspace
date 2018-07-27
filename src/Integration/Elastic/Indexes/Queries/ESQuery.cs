using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Nest;

namespace Elastic.Indexes.Queries
{
    public static class ESQuery
    {
        public static ElasticClient GetClient<TEntity>(this IESIndex<TEntity> index)
        {
            return ((ESIndex<TEntity>)index).Client;
        }
    }
}