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
    public class EmployeesController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult List(int page, int size, string search = null)
        {
            return this.Result(this.Service<IEmployeeManager>().GetEmployeeList(page, size, search));
        }

        [HttpGet("{id}/[action]")]
        public IActionResult GetEmployee(long id)
        {
            return this.Result(this.Service<IEmployeeManager>().GetEmployee(id));
        }

        [HttpPost("[action]")]
        public IActionResult AddEmployee(Employee profile)
        {
            return this.Result(this.Service<IEmployeeManager>().AddEmployee(profile));
        }

        [HttpPut("{id}/[action]")]
        public IActionResult UpdateEmployee(Employee profile)
        {
            return this.Result(this.Service<IEmployeeManager>().UpdateEmployee(profile));
        }

        [HttpDelete("{id}/[action]")]
        public IActionResult DeleteEmployee(long id)
        {
            return this.Result(this.Service<IEmployeeManager>().DeleteEmployee(id));
        }

        [HttpPost("{id}/[action]")]
        public IActionResult AddProject(Project project) 
        {
            return this.Result(this.Service<IEmployeeManager>().AddProject(project));
        }

        [HttpDelete("{id}/[action]/{projectId}")]
        public IActionResult DeleteProject(long id, long projectId) 
        {
            return this.Result(this.Service<IEmployeeManager>().DeleteProject(id, projectId));
        }             
    }
}
