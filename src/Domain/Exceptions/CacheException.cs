namespace Domain.Exceptions;

public class CacheException : Exception
{
    public CacheException(string? message = "") : base(message) { }
}
