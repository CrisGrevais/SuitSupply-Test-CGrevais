using Domain.Contracts.Errors;
using Domain.Errors;
using System;
using System.Linq;
using System.Net.Http;
using WebAPI.Client.Models.Input;
using WebAPI.Client.Models.Output;
using WebAPI.Client.Service.RestClient;

namespace WebAPI.Client.Service
{
    public class ExternalServiceBase : IExternalService
    {
        protected readonly IRestWebClient _webClient;
        protected readonly IErrorCollector _errorCollector;

        public ExternalServiceBase(IErrorCollector errorCollector, IRestWebClient webClient)
        {
            if (errorCollector == null || webClient == null)
            {
                throw new ArgumentNullException("External Service dependencies cannot be null.");
            }

            _webClient = webClient;
            _errorCollector = errorCollector;
        }

        public string ServiceUrl { get; set; }

        #region Protected

        protected GenericResponse<O> CallWebClient<I, O>(ExternalServiceCallParameter<I> input)
        {
            HttpResponseMessage response = InvokeMethod(input);

            if (response == null)
            {
                return GetGenericFailResponse<O>();
            }

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericResponse<O>>(GetResultFromResponse(response));

            if (result != null && result.Code == 401)
            {
                var error = result.Errors.FirstOrDefault(x => x.Code == "Unauthorized");
                if (error != null)
                {
                    _errorCollector.Errors.Add(new ErrorMessage(error.Code, error.Message));
                }
            }

            return result;
        }

        protected string GetResultFromResponse(HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        private HttpResponseMessage InvokeMethod<I>(ExternalServiceCallParameter<I> input)
        {
            var apiUrl = input.APIUrl;
            var request = new RestWebRequest<I>
            {
                Method = input.ApiMethod,
                RequestBody = input.Request,
                ServiceBaseUrl = apiUrl,
                MediaType = input.MediaType
            };

            ServiceUrl = apiUrl + request.Method;

            return input.WebCall.Invoke(request);
        }

        protected GenericResponse<O> GetGenericFailResponse<O>()
        {
            return new GenericResponse<O>
            {
                Code = 400,
                Errors = new[]
                {
                    new ErrorMessage("NoResponse", "No Response back from service.", ErrorTypes.Processing)
                }
            };
        }

        protected ExternalServiceCallParameter<I> CreateServiceCallParameter<I>(I input, Func<RestWebRequest<I>, HttpResponseMessage> webCall, string apiMethod, string serviceUrl)
        {
            return new ExternalServiceCallParameter<I>
            {
                APIUrl = serviceUrl,
                Request = input,
                WebCall = webCall,
                ApiMethod = apiMethod
            };
        }

        #endregion
    }
}
