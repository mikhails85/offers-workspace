using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Results;

namespace Contracts
{
    public interface IStatisticProvider
    {
        Result<TotalDocuments> GetTotalDocuments();
        Result<Dictionary<string,long>> GetOffersSkills();
        Result<Dictionary<string,long>> GetEmployeesSkills();
    }
}