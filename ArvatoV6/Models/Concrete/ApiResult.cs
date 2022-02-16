namespace ArvatoV6.Models.Concrete;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public List<string> Message { get; set; }
    public string Data { get; set; }
}