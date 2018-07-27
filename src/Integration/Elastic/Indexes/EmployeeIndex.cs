using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elastic.Indexes.Models;
using Models;

namespace Elastic.Indexes
{
    public class EmployeeIndex:  ESIndex<Employee>
    {
        private const string Index ="employees";
        public EmployeeIndex(ElasicStorage storage)        
        :base(storage, Index)
        {            
        }
        protected override void CreateIndex()
        {
            var createIndexResponse = this.Client.CreateIndex(Index, c => c
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(0)
                )
                .Mappings(m => m                    
                    .Map<ESEmployee>(mm => mm
                        .AutoMap()                        
                    )
                )
            );

            if(!createIndexResponse.IsValid)
            {
                throw new Exception(createIndexResponse.DebugInformation);
            }
        }
    }
}