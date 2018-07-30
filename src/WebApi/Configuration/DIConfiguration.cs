using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts;
using Contracts.Integration;
using Elastic;
using Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql;
using RabbitMq;
using Services;

namespace WebApi.Configuration
{
    public static class DIConfiguration
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration config)
        {
            services
            .AddSingleton<ILogger, ConsoleLogger>()
            .AddTransient<IEmployeeManager,EmployeeManager>()
            .AddTransient<IOfferManager, OfferManager>()
            .AddTransient<ISkillManager,SkillManager>()
            .AddTransient<IESStorage,ESStorage>()
            .AddTransient<IDbContext,UnitOfWork>()
            .AddTransient<IQueueManager,MQManager>()
            .AddSingleton<MQSettings>((ctx) => ConfigurateSettings<MQSettings>(config,"MQSettings"))
            .AddSingleton<ElasticSettings>((ctx) => ConfigurateSettings<ElasticSettings>(config,"ESSettings"))
            .AddSingleton<DbSettings>((ctx) => ConfigurateSettings<DbSettings>(config,"MySQLSettings"));            
        }

        private static T ConfigurateSettings<T>(IConfiguration config, string section) where T:new()
        {
            var settings = new T(); 
            config.GetSection(section).Bind(settings);
            return settings;    
        }
    }
}