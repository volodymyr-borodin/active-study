namespace ActiveStudy.Domain
{
    public class DomainError
    {
        public string Message { get; }
        public string InternalError { get; }
        public ErrorCode Code { get; }

        public DomainError(string message) : this(message, string.Empty)
        { }

        public DomainError(string message, string internalError) : this(message, internalError, ErrorCode.Undefined)
        { }

        public DomainError(string message, string internalError, ErrorCode code)
        {
            Message = message;
            InternalError = internalError;
            Code = code;
        }
    }

    public enum ErrorCode
    {
        Undefined = 400,
        AccessDenied = 403
    }
}
