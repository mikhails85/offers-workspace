using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Employee
    {
        public long Id {get;set;}
        public string Name {get;set;}
        public string JobTitle {get;set;}
        public string Photo {get;set;}
        public string CV {get;set;}                                       
        public List<Project> Projects {get;set;}
    }
}