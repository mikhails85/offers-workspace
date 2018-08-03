using System;
using System.Collections.Generic;
using Nest;

namespace ES.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node).DefaultIndex("test");
            var client = new ElasticClient(settings);
            if(client.IndexExists("test").Exists)
            {
                client.DeleteIndex("test");
            }
            if(!client.IndexExists("test").Exists)
            {
                var createResult = client.CreateIndex("test", c => c
                    .Settings(s => s
                        .NumberOfShards(1)
                        .NumberOfReplicas(0)
                    )
                    .Mappings(m => m                    
                        .Map<Model>(mm => mm
                            .AutoMap()
                        )
                    )
                );

                if (!createResult.IsValid)
                {
                    Console.WriteLine($"Error: {createResult.DebugInformation}");                   
                    return;
                }
            }

            var result = client.IndexDocument(new Model{Id = 1, Name ="Test 1", Skills= new List<string>{"C#", ".Net Core", "Vue.Js"}});
            if (!result.IsValid)
                Console.WriteLine($"Error: {result.DebugInformation}"); 
                
            result = client.IndexDocument(new Model{Id = 2, Name ="Test 2", Skills= new List<string>{"C#", ".Net Core", "Vue.Js"}});
            if (!result.IsValid)
                Console.WriteLine($"Error: {result.DebugInformation}"); 
                
            var searchResponse = client.Search<Model>(s => s                
                .Size(0)
                .Aggregations(x=>x.Terms("skills", p=>p.Field(f=>f.Skills)))
            );
            
            var people = searchResponse.Aggregations.Terms("skills").Buckets; 
            
            foreach(var item in people)
            {
                Console.WriteLine($"Bucket: {item.Key}:{item.DocCount}");    
            }
        }
    }

    public class Model
    {
        public int Id { get;set;}
        public string Name { get;set;}
        public List<string> Skills { get;set;}
    }
}
