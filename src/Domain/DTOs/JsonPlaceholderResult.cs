using System.Net;

namespace Application.DTOs;

public class JsonPlaceholderResult<T> where T : class
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }
}
