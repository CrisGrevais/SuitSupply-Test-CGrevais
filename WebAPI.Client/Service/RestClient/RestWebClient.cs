using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Client.Models.Input;

namespace WebAPI.Client.Service.RestClient
{
    public class RestWebClient : IRestWebClient
    {
        private const string mediaType = "application/x-www-form-urlencoded";

        public Task<HttpResponseMessage> Post<T>(RestWebRequest<T> request)
        {
            return CallWebService<T>(request, (client, req) => client.PostAsJsonAsync(req.Method, req.RequestBody));
        }

        public Task<HttpResponseMessage> Get<T>(RestWebRequest<T> request)
        {
            return CallWebService<T>(request, (client, req) => client.GetAsync(req.Method));
        }

        public Task<HttpResponseMessage> Put<T>(RestWebRequest<T> request)
        {
            return CallWebService<T>(request, (client, req) => client.PutAsJsonAsync(req.Method, req.RequestBody));
        }

        public Task<HttpResponseMessage> Delete<T>(RestWebRequest<T> request)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request.RequestBody), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(request.ServiceBaseUrl + request.Method)
            };

            return CallWebService<T>(request, (client, req) => client.SendAsync(requestMessage));
        }

        #region Private

        protected async Task<HttpResponseMessage> CallWebService<T>(RestWebRequest<T> request, Func<HttpClient, RestWebRequest<T>, Task<HttpResponseMessage>> action)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.ServiceBaseUrl) ||
                string.IsNullOrWhiteSpace(request.Method))
            {
                throw new ArgumentNullException("Invalid Request.");
            }

            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(request.ServiceBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", string.IsNullOrWhiteSpace(request.MediaType) ? mediaType : request.MediaType);

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                response = action.Invoke(client, request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    HttpError error = null;
                    response.TryGetContentValue(out error);
                }
            }

            return response;
        }

        #endregion
    }
}
