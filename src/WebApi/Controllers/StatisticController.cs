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
    public class StatisticController : ControllerBase
    {
        // GET api/values
        [HttpGet("[action]")]
        public IActionResult TotalDocs()
        {
            return this.Result(this.Service<IStatisticProvider>().GetTotalDocuments());
        }
    }
}
