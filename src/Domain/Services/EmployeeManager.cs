using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts;
using Contracts.Integration;
using Elastic.Indexes.Queries;
using Models;
using MySql.Queries;
using Results;
using Wrappers;

namespace Services
{
    public class EmployeeManager: IEmployeeManager
    {
        private readonly IDbContext context;
        private readonly IQueueManager queue;
        private readonly IESStorage esstorage;

        public EmployeeManager (IDbContext context, IQueueManager queue, IESStorage esstorage)
        {
             this.context = context;
             this.queue = queue;   
             this.esstorage = esstorage;
        }

        public VoidResult AddEmployee(Employee profile)
        {
            var result = this.context.Query(new AddEmployee(profile));
            if(result.Success)
            {
                context.Save();
                profile.Id = result.Value;
                queue.SendMessage("employees", new CRUDWrapper<Employee>{ Action = CRUDActionType.Create, Entity = profile });
            }            
            return result;
        }

        public VoidResult AddProject(Project project)
        {
            var result = this.GetEmployee(project.Id);            
            if(!result.Success)
            {
                return result;
            }

            var profile = result.Value;

            var projectResult = this.context.Query(new AddProject(project));
            if(projectResult.Success)
            {
                project.Id = projectResult.Value;

                foreach(var skill in project.UsedSkills)
                {
                   this.context.Query(new AddProjectSkill(project.Id, skill.Id));     
                }

                context.Save();
                profile.Projects.Add(project);
                queue.SendMessage("employees", new CRUDWrapper<Employee>{ Action = CRUDActionType.Update, Entity = profile });
            }           

            return projectResult;            
        }

        public VoidResult DeleteEmployee(long id)
        {
            var result = this.GetEmployee(id);            
            if(!result.Success)
            {
                return result;
            }

            var profile = result.Value;

            foreach(var proj in profile.Projects)
            {
                var delSkillResult = this.context.Query(new DeleteProjectSkills(proj.Id));
                if(!delSkillResult.Success)
                {
                    return delSkillResult;
                }    
            }

            var delProjResult = this.context.Query(new DeleteEmployeeProjects(id));
            if(!delProjResult.Success)
            {
                return delProjResult;
            }

            var delEmplResult = this.context.Query(new DeleteEmployee(id));
            if(!delEmplResult.Success)
            {
                return delEmplResult;
            }

            this.context.Save();
            queue.SendMessage("employees", new CRUDWrapper<Employee>{ Action = CRUDActionType.Delete, Entity = profile });

            return delEmplResult;
        }

        public VoidResult DeleteProject(long id, long projectId)
        {
            var result = this.GetEmployee(id);            
            if(!result.Success)
            {
                return result;
            }

            var profile = result.Value;

            
            var delSkillResult = this.context.Query(new DeleteProjectSkills(projectId));
            if(!delSkillResult.Success)
            {
                return delSkillResult;
            }    
            

            var delProjResult = this.context.Query(new DeleteProject(projectId));
            if(!delProjResult.Success)
            {
                return delProjResult;
            }

            this.context.Save();

            profile.Projects = profile.Projects.Where(x=>x.Id != projectId).ToList();             
            queue.SendMessage("employees", new CRUDWrapper<Employee>{ Action = CRUDActionType.Update, Entity = profile });
            return delProjResult;
        }

        public VoidResult UpdateEmployee(Employee employee)
        {
            var result = this.GetEmployee(employee.Id);            
            if(!result.Success)
            {
                return result;
            }

            var profile = result.Value;
            profile.JobTitle = employee.JobTitle;
            profile.Name = employee.Name;
            profile.Photo = employee.Photo;
            profile.CV = employee.CV;
            
            var updateResult = this.context.Query(new UpdateEmployee(profile));
            if(updateResult.Success)
            {
                context.Save();
                queue.SendMessage("employees", new CRUDWrapper<Employee>{ Action = CRUDActionType.Update, Entity = profile });
            }            
            return updateResult;
        }

        public Result<Employee> GetEmployee(long id)
        {
            return this.context.Query(new GetEmployee(id));
        }
        
        public Result<IEnumerable<Employee>> GetEmployeeList(int page, int size, string search)
        {
            return this.esstorage.Get<Employee>().Query(new SearchEmployees(page, size, search));
        }
    }
}