using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Dapper;
using Models;
using Results;
using Results.Errors;

namespace MySql.Queries
{
    public class GetEmployee: Result<Employee>, IDbQuery
    {
        private readonly long employeeId;
        private Dictionary<long, Employee> employees;
        public GetEmployee(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();        
                
                employees = new Dictionary<long, Employee>();

                dbConnection.Query<Employee, Project, Skill, Employee>(@"SELECT Employees.*, Projects.*, Skills.*
                            FROM Employees
                            LEFT JOIN Projects ON Employees.Id = Projects.EmployeesId
                            LEFT JOIN ProjectsSkills ON Projects.Id = ProjectsSkills.ProjectId
                            LEFT JOIN Skills ON ProjectsSkills.SkillId = Skills.Id
                            WHERE Employees.Id = @Id;", Mapping, new {Id = employeeId });  

                base.SetValue(employees[employeeId]);
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }

        private Employee Mapping(Employee employee, Project project, Skill skill)
        {
            Employee e;

            if (!employees.TryGetValue(employee.Id, out e))
            {             
                e = employee;
                e.Projects = new List<Project>();
                employees.Add(e.Id,e);
            }

            if (e.Projects.All(p => p.Id != project.Id))
            {
                project.UsedSkills = new List<Skill>();
                e.Projects.Add(project);
            }

            var proj = e.Projects.First(p => p.Id == project.Id);

            if (proj.UsedSkills.All(s => s.Id != skill.Id))
            {
                proj.UsedSkills.Add(skill);
            }
            return e;
        }
    }
}