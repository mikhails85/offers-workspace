using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Offer
    {
        public long Id {get;set;}
        public string Name {get; set;}
        public string Description{get; set;}
        public List<Skill> RequaredSkills {get; set;}        
    }
}