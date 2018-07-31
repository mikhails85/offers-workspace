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
        public List<string> Skills {get;set;}

        public static implicit operator ESEmployee(Employee employee)
        {
            ESEmployee e = new ESEmployee();            
            e.Id = employee.Id;
            e.Name = employee.Name;
            e.JobTitle = employee.JobTitle;
            e.Photo = employee.Photo;
            e.CV = employee.CV;
            e.Projects = employee.Projects;

            e.Skills = employee.Projects?.SelectMany(x=>x.UsedSkills).GroupBy(x=>x.Name).Select(x=>x.Key).ToList();
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