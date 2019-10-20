namespace Domain.Contracts.Errors
{
    public interface IErrorMessage
    {
        string Message { get; set; }
        string Code { get; set; }
        ErrorTypes Type { get; set; }
    }
}
