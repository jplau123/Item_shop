namespace ND_2023_12_19.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message) : base(message) { }
}
