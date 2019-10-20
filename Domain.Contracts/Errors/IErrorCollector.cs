using System.Collections.Generic;

namespace Domain.Contracts.Errors
{
    public interface IErrorCollector
    {
        IList<IErrorMessage> Errors { get; }
        bool HasError { get; }
        string[] To();
    }
}
