using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Nest;

namespace Elastic.Indexes
{
    public abstract class ESIndex<TEntity>:IESIndex<TEntity>
    {
        private readonly string index;
        protected ElasticClient client;

        public ElasticClient Client => this.client;

        public ESIndex(ElasicStorage storage, string index )
        {
            this.index = index;
            var node = new Uri(storage.ConnectionSettings.NodeUrl);
            var settings = (new ConnectionSettings(node)).DefaultIndex(index).DisableDirectStreaming();
            this.client = new ElasticClient(settings);

            if(!this.IndexExists())
            {
                this.CreateIndex();     
            }
        }

        protected abstract void CreateIndex();

        protected bool IndexExists(string index = null) 
        {
            return this.client.IndexExists(index ?? this.index).Exists;
        } 

        protected void RemoveIndex(string index = null) 
        {
             this.client.DeleteIndex(index ?? this.index);
        }
        
        protected void PutDocument<T>(T doc) where T:class
        {
            this.client.IndexDocument(doc);
        }
        
        protected void RemoveDocument<T>(string id) where T:class
        {
            this.client.Delete<T>(id);
        }

        public TQuery Query<TQuery>(TQuery query) where TQuery : IESQuery<TEntity>
        {
            query.Execute(this);
            return query;
        }
    }
}