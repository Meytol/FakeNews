using FakeNews.Common.StaticMethods;
using System.Net;

namespace FakeNews.Common.Models
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {

        }

        public ServiceResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            IsSuccessful = statusCode.IsSuccessfull();
        }

        public bool IsSuccessful { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public ServiceResponse(T data) : base()
        {
            Data = data;
        }

        public ServiceResponse(T data, HttpStatusCode statusCode) : base()
        {
            Data = data;
            StatusCode = statusCode;
            IsSuccessful = statusCode.IsSuccessfull() && Data is not null;
        }

        public T Data { get; set; }
    }

}
