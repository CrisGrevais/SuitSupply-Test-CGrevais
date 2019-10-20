namespace WebAPI.Client.Models.Input
{
    public class RestWebRequest<T>
    {
        public string ServiceBaseUrl { get; set; }
        public string Method { get; set; }
        public T RequestBody { get; set; }
        public string MediaType { get; set; }
    }
}
