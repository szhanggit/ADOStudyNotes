using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTXC
{
    public static class Response
    {
        public static Response<T> Fail<T>(string msg, T data) => new Response<T>(data, msg, false);
        public static Response<T> Success<T>(string msg, T data) => new Response<T>(data, msg, true);
    }

    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T data, string msg, bool success)
        {
            Data = data;
            Message = msg;
            Success = success;
        }
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
