namespace ND_2023_12_19.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message) { }
}
