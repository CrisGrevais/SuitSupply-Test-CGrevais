using Domain.Contracts.Errors;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Errors
{
    public class ErrorCollector : IErrorCollector
    {
        public ErrorCollector()
        {
            this.Errors = new List<IErrorMessage>();
        }

        public IList<IErrorMessage> Errors { get; private set; }

        public bool HasError
        {
            get
            {
                return this.Errors != null && this.Errors.Any();
            }
        }

        public string[] To()
        {
            return this.Errors.Select(x => x.Message).ToArray();
        }
    }
}
