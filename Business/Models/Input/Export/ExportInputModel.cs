using Business.Contracts.Models.Input.Export;
using System.Collections.Generic;
using System.Linq;

namespace Business.Models.Input.Export
{
    public class ExportInputModel<T> : IExportInputModel<T>
    {
        public string EntityName { get; set; }
        public IEnumerable<T> SourceData { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(EntityName) &&
                   SourceData != null && SourceData.Any();
        }
    }
}
