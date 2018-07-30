using System;
using System.IO;
using DatabaseSynchronizer.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseSynchronizer
{
    class Program
    {        
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjection(configuration);
 
            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
 
            // entry to run app
            serviceProvider.GetService<SyncEmployees>().Run();
            serviceProvider.GetService<SyncOffers>().Run();

            Console.WriteLine("Press [Enter] to stop");
            Console.ReadKey();
        }
    }
}
