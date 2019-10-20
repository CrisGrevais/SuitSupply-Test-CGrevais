using System;
using System.Net.Http;

namespace WebAPI.Client.Models.Input
{
    public class ExternalServiceCallParameter<T>
    {
        public T Request { get; set; }
        public Func<RestWebRequest<T>, HttpResponseMessage> WebCall { get; set; }
        public string ApiMethod { get; set; }
        public string APIUrl { get; set; }
        public string MediaType { get; set; }
    }
}
