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

    /// <param name="messageSeparator">If ommited, Environment.NewLine</param>
    /// <returns>If no errors, returns a ResponseCode.Success Response</returns>
    public static Response AggregateErrors(IEnumerable<Response> responses, string? messageSeparator = null)
    {
        messageSeparator ??= Environment.NewLine;

        List<string> messages = new();

        bool error = false;

        foreach(var response in responses)
        {
            if(response.Code != ResponseCode.Success)
            {
                error = true;
                messages.Add(response.Message);
            }
        }

        if(!error)
        {
            return new Response(ResponseCode.Success, "Success.");
        }
        return new Response(ResponseCode.ServiceError, string.Join(messageSeparator, messages));
    }
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
    //Ideally, this is the only one success code, the payload provides extra info
    //If not, fix some implementations mostly here and hopefully not somewhere there.
    Success,

    InvalidField,
    Unauthorized,
    NotFound,
    RepositoryError,
    InvalidOperation,
    ServiceError
}
