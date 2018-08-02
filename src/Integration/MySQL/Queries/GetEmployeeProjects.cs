using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.Integration;
using Dapper;
using Models;
using MySql.Queries;
using Results;
using Results.Errors;

namespace MySql.Queries
{
    public class GetEmployeeProjects: Result<IEnumerable<Project>>, IDbQuery
    {
        private readonly long employeeId;
        private List<Project> projects;
        public GetEmployeeProjects(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public void Execute(IDbContext context)
        {
            try
            {
                var dbConnection = context.GetConnection();        
                
                projects = new List<Project>();

                dbConnection.Query<Project, Skill, Project>(@"SELECT Projects.*, Skills.*
                            FROM Projects                            
                            LEFT JOIN ProjectsSkills ON Projects.Id = ProjectsSkills.ProjectId
                            LEFT JOIN Skills ON ProjectsSkills.SkillId = Skills.Id
                            WHERE Projects.EmployeeId = @Id;", Mapping, new {Id = employeeId });  

                base.SetValue(projects);
            }
            catch(Exception ex)
            {
                base.AddErrors(new UnexpectedError(ex));
            }
        }

        private Project Mapping(Project project, Skill skill)
        {
            Project p;

            if (projects.All(x=>x.Id != project.Id))
            {             
                p = project;
                p.UsedSkills = new List<Skill>();
                projects.Add(p);
            }
            else {
                p = projects.First(x=>x.Id != project.Id);
            }

            if (skill!=null && p.UsedSkills.All(s => s.Id != skill.Id))
            {
                p.UsedSkills.Add(skill);
            }
                        
            return p;
        }
    }
}