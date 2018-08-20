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
        private readonly ISkillManager skillsStorage;

        public StatisticProvider(IESStorage esstorage, ISkillManager skillsStorage)
        {
            this.esstorage = esstorage;
            this.skillsStorage = skillsStorage;
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
            var result = new Result<Dictionary<string,long>>();
            var skillResult = this.skillsStorage.GetSkillList();
            if(!skillResult.Success)
            {
               result.AddErrors(skillResult.Errors);
               return result;      
            }
            var offersSkillsResult = this.esstorage.Get<Offer>().Query(new GetOffersSkills());

            if(!offersSkillsResult.Success)
            {
               result.AddErrors(offersSkillsResult.Errors);
               return result;      
            }

            var statistic = new Dictionary<string, long>();

            foreach(var key in offersSkillsResult.Value.Keys)
            {
                var id = long.Parse(key);
                statistic.Add(skillResult.Value.First(x=>x.Id == id).Name, offersSkillsResult.Value[key]);
            }

            result.SetValue(statistic);

            return result;            
        }

        public Result<Dictionary<string,long>> GetEmployeesSkills()
        {   
            var result = new Result<Dictionary<string,long>>();
            var skillResult = this.skillsStorage.GetSkillList();
            if(!skillResult.Success)
            {
               result.AddErrors(skillResult.Errors);
               return result;      
            }         
            var employeesSkillsResult = this.esstorage.Get<Employee>().Query(new GetEmployeesSkills());     

            if(!employeesSkillsResult.Success)
            {
               result.AddErrors(employeesSkillsResult.Errors);
               return result;      
            }

            var statistic = new Dictionary<string, long>();

            foreach(var key in employeesSkillsResult.Value.Keys)
            {
                var id = long.Parse(key);
                statistic.Add(skillResult.Value.First(x=>x.Id == id).Name, employeesSkillsResult.Value[key]);
            }

            result.SetValue(statistic);

            return result;            
        }
    }
}