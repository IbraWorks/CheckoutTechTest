using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Application
{
    public static class Response
    {
        public static Response<T> Failure<T>(string msg, T data = default) => new Response<T>(data, msg, true);

        public static Response<T> Success<T>(T data, string msg) => new Response<T>(data, msg, false);
    }

    public class Response<T>
    {

        public T Data { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }

        public Response(T data, string msg, bool error)
        {
            Data = data;
            Message = msg;
            Error = error;
        }
    }
}
