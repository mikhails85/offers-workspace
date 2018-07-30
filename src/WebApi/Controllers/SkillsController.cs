using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Configuration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        // GET api/values
        [HttpGet("[action]")]
        public IActionResult List()
        {
            return this.Result(this.Service<ISkillManager>().GetSkillList());
        }

        [HttpPost("[action]")]
        public IActionResult AddSkill([FromBody]Skill skill)
        {
            return this.Result(this.Service<ISkillManager>().AddSkill(skill));
        }
    }
}
