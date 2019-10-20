using Domain.Contracts.Errors;

namespace Domain.Errors
{
    public class ErrorMessage : IErrorMessage
    {
        public ErrorMessage()
        {
        }

        public ErrorMessage(string code, string message)
        {
            this.Code = code;
            this.Message = message;
            this.Type = ErrorTypes.Info;
        }

        public ErrorMessage(string code, string message, ErrorTypes type)
        {
            this.Code = code;
            this.Message = message;
            this.Type = type;
        }

        public ErrorMessage(string code, string message, ErrorTypes type, string errorSource)
            : this(code, message, type)
        {
            ErrorSource = errorSource;
        }

        public string Message { get; set; }
        public string Code { get; set; }
        public ErrorTypes Type { get; set; }
        public string ErrorSource { get; set; }
    }
}
