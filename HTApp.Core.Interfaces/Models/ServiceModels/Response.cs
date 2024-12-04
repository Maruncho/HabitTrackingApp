namespace HTApp.Core.API;

public class Response
{
    public Response(ResponseCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public ResponseCode Code { get; init; }
    public string Message { get; init; } = null!;
}

public class Response<PayloadType> where PayloadType : class?
{
    public Response(ResponseCode code, string message, PayloadType? payload = null)
    {
        Code = code;
        Message = message;
        Payload = payload;
    }

    public ResponseCode Code { get; init; }
    public string Message { get; init; } = null!;
    public PayloadType? Payload { get; init; }
}

public class ResponseStruct<PayloadType> where PayloadType : struct
{
    public ResponseStruct(ResponseCode code, string message, PayloadType payload = default)
    {
        Code = code;
        Message = message;
        Payload = payload;
    }

    public ResponseCode Code { get; init; }
    public string Message { get; init; } = null!;
    public PayloadType Payload { get; init; }
}

public enum ResponseCode
{
    Success,
    InvalidField,
    Unauthorized,
    NotFound,
    RepositoryError
}
