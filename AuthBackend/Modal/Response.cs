
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null, int responseCode = 0, string notification = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            ResponseCode = responseCode;
            Notification = notification;
        }
        public Response(T data, bool isSucceeded, string message = null, int responseCode = 0)
        {
            Succeeded = isSucceeded;
            Message = message;
            Data = data;
            ResponseCode = responseCode;
        }
        public Response(string message, int responseCode = 0, string title = null)
        {
            Succeeded = false;
            Message = message;
            ResponseCode = responseCode;
            Title = title;
        }



        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Notification { get; set; }
        public string Title { get; set; }
        public int ResponseCode { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }

     

    public class Responses<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; } 
        public int ResponseCode { get; set; } = 200;  
    }


}
