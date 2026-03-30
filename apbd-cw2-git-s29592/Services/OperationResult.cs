namespace apbd_cw2_git_s29592.Services;

public record OperationResult(bool IsSuccess, string? Error = null)
{
    public static OperationResult Ok() => new(true);
    public static OperationResult Fail(string msg) => new(false, msg);
}

public record OperationResult<T>(bool IsSuccess, T? Value = default, string? Error = null)
{
    public static OperationResult<T> Ok(T value) => new(true, value);
    public static OperationResult<T> Fail(string msg) => new(false, default, msg);
}