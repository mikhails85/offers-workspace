using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Elastic.Indexes.Models
{
    public class ESEmployee
    {
        public long Id {get;set;}
        public string Name {get;set;}
        public string JobTitle {get;set;}
        public string Photo {get;set;}
        public string CV {get;set;}                                       
        public List<Project> Projects {get;set;}
        public List<Skill> Skills {get;set;}

        public static implicit operator ESEmployee(Employee employee)
        {
            ESEmployee e = new ESEmployee();            
            e.Id = employee.Id;
            e.Name = employee.Name;
            e.JobTitle = employee.JobTitle;
            e.Photo = employee.Photo;
            e.CV = employee.CV;
            e.Projects = employee.Projects;

            var skills = employee.Projects?.SelectMany(x=>x.UsedSkills).ToList();
            e.Skills = skills.GroupBy(x=>x.Id).Select(x=>skills.First(s=>s.Id==x.Key)).ToList();
            return e;
        }

        public static implicit operator Employee(ESEmployee employee)
        {
            Employee e = new Employee();            
            e.Id = employee.Id;
            e.Name = employee.Name;
            e.JobTitle = employee.JobTitle;
            e.Photo = employee.Photo;
            e.CV = employee.CV;
            e.Projects = employee.Projects;
            
            return e;
        }
    }
}