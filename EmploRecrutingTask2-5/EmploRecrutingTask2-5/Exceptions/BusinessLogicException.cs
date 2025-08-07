namespace EmploRecrutingTask2_5.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string message) : base(message) { }
    public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }
}
