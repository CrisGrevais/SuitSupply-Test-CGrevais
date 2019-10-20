namespace Domain.Contracts.Errors
{
    public enum ErrorTypes
    {
        Validation = 0,
        NotProcessingAllowed = 1,
        Processing = 2,
        Info = 3,
        Warning = 4,
        ValidFailure = 5,
        Timeout = 6
    }
}
