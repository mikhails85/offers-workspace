using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts;
using Contracts.Integration;
using Elastic.Indexes.Queries;
using Models;
using Results;

namespace Services
{
    public class StatisticProvider : IStatisticProvider
    {
        private readonly IESStorage esstorage;

        public StatisticProvider(IESStorage esstorage)
        {
            this.esstorage = esstorage;
        }

        public Result<TotalDocuments> GetTotalDocuments()
        {
            var result = new Result<TotalDocuments>();
            var totalDocuments = new TotalDocuments();
            var offerResult = this.esstorage.Get<Offer>().Query(new GetTotalOffers());
            if(!offerResult.Success)
            {
                result.AddErrors(offerResult.Errors);
                return result;
            }
            totalDocuments.TotalOffers = offerResult.Value;

            var employeeResult = this.esstorage.Get<Employee>().Query(new GetTotalEmployees());
            if(!employeeResult.Success)
            {
                result.AddErrors(employeeResult.Errors);
                return result;
            }            
            totalDocuments.TotalEmployees = employeeResult.Value;
            result.SetValue(totalDocuments);

            return result;
        }

        public Result<Dictionary<string,long>> GetOffersSkills()
        {
            return this.esstorage.Get<Offer>().Query(new GetOffersSkills());            
        }

        public Result<Dictionary<string,long>> GetEmployeesSkills()
        {            
            return this.esstorage.Get<Employee>().Query(new GetEmployeesSkills());            
        }
    }
}