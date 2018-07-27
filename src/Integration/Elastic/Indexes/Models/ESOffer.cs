using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Nest;

namespace Elastic.Indexes.Models
{
    public class ESOffer
    {
        public long Id {get;set;}
        public string Name {get; set;}
        public string Description{get; set;}
        public List<Skill> RequaredSkills {get; set;}        
        public QueryContainer Query { get; set; }

        public static implicit operator ESOffer(Offer offer)
        {
            ESOffer o = new ESOffer();            
            o.Id = offer.Id;
            o.Name = offer.Name;
            o.Description = offer.Description;
            o.RequaredSkills = offer.RequaredSkills;

            foreach(var skill in offer.RequaredSkills)    
            {
                o.Query &= new MatchQuery 
                {
                    Field = Infer.Field<ESEmployee>(entry => entry.Skills),
                    Query = skill.Name
                };    
            }
            
            return o;
        }

        public static implicit operator Offer(ESOffer offer)
        {
            Offer o = new Offer();            
            o.Id = offer.Id;
            o.Name = offer.Name;
            o.Description = offer.Description;
            o.RequaredSkills = offer.RequaredSkills;
            
            return o;
        }
    }
}