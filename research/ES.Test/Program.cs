using System;
using System.Collections.Generic;
using Nest;
using System.Linq;
namespace ES.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node).DefaultIndex("employees");
            var client = new ElasticClient(settings);
            if(client.IndexExists("employees").Exists)
            {
                client.DeleteIndex("employees");
                Console.WriteLine($"Index Deleted");           
            }
            
            return;
            
            if(!client.IndexExists("test1").Exists)
            {
                var createResult = client.CreateIndex("test1", c => c
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
                    Console.WriteLine($"Error1: {createResult.DebugInformation}");                   
                    return;
                }
            }

            var result = client.IndexDocument(new Model{Id = 1, Name ="Test 1", Skills= new List<Skill>{new Skill{Name ="C#"}}});
            if (!result.IsValid)
                Console.WriteLine($"Error2: {result.DebugInformation}"); 
                
            result = client.IndexDocument(new Model{Id = 2, Name ="Test 2", Skills= new List<Skill>{new Skill{Name ="C#"}}});
            if (!result.IsValid)
                Console.WriteLine($"Error3: {result.DebugInformation}"); 
                
            var searchResponse = client.Search<Model>(s => s     
                .Size(100)
                .Aggregations(childAggs => childAggs
                    .Nested("project_tags", n => n
                        .Path(p => p.Skills)
                        .Aggregations(nestedAggs => nestedAggs
                            .Terms("tags", avg => avg.Field(p => p.Skills.First().Name))
                        )
                    )
                )
            );
            
             if (!searchResponse.IsValid)
                Console.WriteLine($"Error4: {searchResponse.DebugInformation}"); 
            
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
        
        public List<Skill> Skills { get;set;}
    }
    public class Skill
    {
        public string Name{get;set;}
    }
}
