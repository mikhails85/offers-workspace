using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Results
{
    public class VoidResult
    {
        private readonly List<Error> errors;

        public VoidResult()
        {
            errors = new List<Error>();
        }

        public Error[] Errors => errors.ToArray();

        public virtual bool Success => !Errors.Any();

        public void AddErrors(params Error[] errors) => this.errors.AddRange(errors);
    }
}