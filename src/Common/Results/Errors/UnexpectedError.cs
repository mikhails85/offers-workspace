using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Results.Errors
{
    public class UnexpectedError: Error
    {
        public UnexpectedError(Exception ex)
            : base((int) ErrorCode.Unexpected, ex.ToString())
        {
        }
    }
}