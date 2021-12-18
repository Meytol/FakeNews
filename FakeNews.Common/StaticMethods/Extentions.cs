using MD.PersianDateTime;
using System;
using System.Net;

namespace FakeNews.Common.StaticMethods
{
    public static class Extentions
    {
        public static bool IsSuccessfull(this HttpStatusCode statusCode)
        {
            //1xx is for information
            //2xx is for successfull 
            //3xx is for redirect 
            //4xx is for client error
            //5xx is for server error
            return (int)statusCode < 400;
        }

        public static PersianDateTime AsPersianDateTime(this DateTime dateTime) =>
            new(dateTime);
    }
}
