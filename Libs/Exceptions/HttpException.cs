
namespace Libs.Exceptions;

public class HttpException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
