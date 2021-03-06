using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Results;

namespace Contracts
{
    public interface ISkillManager
    {
        Result<IEnumerable<Skill>> GetSkillList();

        VoidResult AddSkill(Skill skill);
    }
}