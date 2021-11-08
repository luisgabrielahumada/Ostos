using System.Net;
using Filter;

namespace Helper
{
    [SwaggerExclude]
    public class HttpMessage<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public T Data { get; set; }
        
    }

    [SwaggerExclude]
    public class HttpMessageError<T>
    {
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public string Stack { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
    }
}