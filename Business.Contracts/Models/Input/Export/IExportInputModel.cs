using Domain.Contracts.Entities;
using System.Collections.Generic;

namespace Business.Contracts.Models.Input.Export
{
    public interface IExportInputModel<T> : IValidable
    {
        string EntityName { get; set; }
        IEnumerable<T> SourceData { get; set; }
    }
}
