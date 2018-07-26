using System;
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

            var result = client.IndexDocument(new Model{Id = 1, Name ="Test"});
            if (!result.IsValid)
                Console.WriteLine($"Error: {result.DebugInformation}"); 
                
            result = client.IndexDocument(new Model{Id = 1, Name ="Test"});
            if (!result.IsValid)
                Console.WriteLine($"Error: {result.DebugInformation}"); 
                
            var searchResponse = client.Search<Model>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                     .Match(m => m
                        .Field(f => f.Name)
                        .Query("Test")
                     )
                )
            );
            
            var people = searchResponse.Documents; 
            
            foreach(var item in people)
            {
                Console.WriteLine($"Error: {item.Name}");    
            }
        }
    }

    public class Model
    {
        public int Id { get;set;}
        public string Name { get;set;}
    }
}
