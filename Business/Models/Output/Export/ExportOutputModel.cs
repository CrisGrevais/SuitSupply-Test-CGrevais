using Business.Contracts.Models.Output.Export;

namespace Business.Models.Output.Export
{
    public class ExportOutputModel : IExportOutputModel
    {
        public byte[] File { get; set; }
    }
}
