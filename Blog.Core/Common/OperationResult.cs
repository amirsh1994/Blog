namespace Blog.Core.Common;

public class OperationResult
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = "";

    public int Code { get; set; }

}


public class OperationResult<TData>
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = "";

    public int Code { get; set; }

    public TData Data { get; set; }
}