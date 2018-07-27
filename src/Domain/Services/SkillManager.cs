using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts;
using Contracts.Integration;
using Models;
using MySql.Queries;
using Results;

namespace Services
{
    public class SkillManager: ISkillManager
    {
        private readonly IDbContext context;
        private readonly IQueueManager queue;

        public SkillManager (IDbContext context, IQueueManager queue)
        {
             this.context = context;
             this.queue = queue;   
        }

        public Result<IEnumerable<Skill>> GetSkillList()
        {
            return this.context.Query(new GetSkills());
        }

        public VoidResult AddSkill(Skill skill)
        {
            var result = this.context.Query(new AddSkill(skill));
            
            if(result.Success)
            {
                this.context.Save();
            }
            
            return result;
        }
    }
}