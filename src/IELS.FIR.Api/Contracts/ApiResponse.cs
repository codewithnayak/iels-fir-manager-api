namespace IELS.FIR.Api.Contracts;

public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    IReadOnlyList<ApiError> Errors,
    string? TraceId = null)
{
    public static ApiResponse<T> Ok(
        T data,
        string? traceId = null) =>
        new(true, data, [], traceId);

    public static ApiResponse<T> Fail(
        IEnumerable<ApiError> errors,
        string? traceId = null) =>
        new(false, default, [.. errors], traceId);
}