﻿using System;
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

        [HttpGet("[action]")]
        public IActionResult EmployeesSkills()
        {
            return this.Result(this.Service<IStatisticProvider>().GetEmployeesSkills());
        }

        [HttpGet("[action]")]
        public IActionResult OffersSkills()
        {
            return this.Result(this.Service<IStatisticProvider>().GetOffersSkills());
        }

        [HttpGet("[action]/{id}")]
        public IActionResult AvailableEmployees(long id, int page, int size)
        {
            return this.Result(this.Service<IStatisticProvider>().GetAvailableEmployees(page, size,id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult AvailableOffers(long id, int page, int size)
        {
            return this.Result(this.Service<IStatisticProvider>().GetAvailableOffers(page, size,id));
        }
    }
}
