using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Client.Models.Input;

namespace WebAPI.Client.Service.RestClient
{
    public interface IRestWebClient
    {
        Task<HttpResponseMessage> Post<T>(RestWebRequest<T> request);
        Task<HttpResponseMessage> Get<T>(RestWebRequest<T> request);
        Task<HttpResponseMessage> Put<T>(RestWebRequest<T> request);
        Task<HttpResponseMessage> Delete<T>(RestWebRequest<T> request);
    }
}
