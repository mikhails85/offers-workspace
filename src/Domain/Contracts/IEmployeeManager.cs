using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Results;

namespace Contracts
{
    public interface IEmployeeManager
    {
        VoidResult AddEmployee(Employee profile);
        VoidResult AddProject(Project project);
        VoidResult DeleteEmployee(long id);
        VoidResult DeleteProject(long id, long projectId);
        VoidResult UpdateEmployee(Employee employee);
        Result<Employee> GetEmployee(long id);
        Result<IEnumerable<Project>> GetEmployeeProjects(long id);
        Result<IEnumerable<Employee>> GetEmployeeList(int page, int size, string search);
    }
}