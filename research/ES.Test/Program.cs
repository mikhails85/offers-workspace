using System;
using Nest;

namespace ES.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var node = new Uri("http://0.0.0.0:9200");
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
                    Console.WriteLine($"Error: {createResult.DebugInformation}");                   
            }

            var result = client.IndexDocument(new Model{Id = 1, Name ="Test"});
            if (!result.IsValid)
                Console.WriteLine($"Error: {result.DebugInformation}");               
        }
    }

    public class Model
    {
        public int Id { get;set;}
        public string Name { get;set;}
    }
}
