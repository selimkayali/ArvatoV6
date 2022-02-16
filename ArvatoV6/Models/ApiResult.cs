namespace ArvatoV6.Models;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}