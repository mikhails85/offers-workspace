using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Project
    {
        public long Id {get;set;}
        public long EmployeeId {get;set;}
        public string Name{get;set;}
        public string Description{get;set;}        
        public List<Skill> UsedSkills {get;set;}
    }
}