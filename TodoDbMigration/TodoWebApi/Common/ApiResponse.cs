namespace TodoWebApi.Common;

public class ApiResponse<T>
{
    public int Status { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}

public class ApiResponse : ApiResponse<object>
{
}
