namespace Hrms.Application.Common.Exceptions;

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message, string? errorCode = null)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public string? ErrorCode { get; }
}
