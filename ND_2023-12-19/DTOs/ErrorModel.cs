namespace ND_2023_12_19.DTOs;

public class ErrorModel
{
    public int Status { get; set; }
    public string? Message { get; set; } = string.Empty;
    public string? Trace { get; set; } = string.Empty;

}
