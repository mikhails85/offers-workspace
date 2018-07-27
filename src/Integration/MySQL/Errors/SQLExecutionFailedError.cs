using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Results;

namespace MySql.Errors
{
    public class SQLExecutionFailedError: Error
    {
        public SQLExecutionFailedError(string message) 
            : base((int)ErrorCode.SQLExecutionFailed, message)
        {
        }
    }
}