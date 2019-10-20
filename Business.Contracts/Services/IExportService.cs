using Business.Contracts.Models.Input.Export;
using Business.Contracts.Models.Output.Export;

namespace Business.Contracts.Services
{
    public interface IExportService
    {
        IExportOutputModel Export<T>(IExportInputModel<T> inputModel);
    }
}
