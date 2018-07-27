using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;

namespace Elastic
{
    public abstract class ElasicStorage: IESStorage
    {
        private readonly ElasticSettings settings;
        
        private readonly IDictionary<Type, object> resolvers;
        
        public ElasticSettings ConnectionSettings => this.settings;

        public ElasicStorage(ElasticSettings settings)
        {
            this.settings = settings;
            this.resolvers = new Dictionary<Type, object>();
            this.OnInitEEntityResolvers();      
        }
        
        public virtual IESIndex<TEntity> Get<TEntity>()
        {
            var res = (Func<ElasicStorage,IESIndex<TEntity>>)this.resolvers[typeof(TEntity)];            
            return res(this);
        }
        
        protected abstract void OnInitEEntityResolvers();      
        
        protected virtual void Set<TEntity>(Func<ElasicStorage,IESIndex<TEntity>> resolver)
        {
            this.resolvers.Add(typeof(TEntity), resolver);
        }
    }
}