using Domain.Contracts.Errors;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ApiControllerBase : ApiController
    {
        protected IErrorCollector ErrorCollector { get; private set; }

        public ApiControllerBase(IErrorCollector errorCollector)
        {
            if (errorCollector == null)
            {
                throw new ArgumentNullException("ApiControllerBase dependencies cannot be null.");
            }

            ErrorCollector = errorCollector;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            response = await base.ExecuteAsync(controllerContext, cancellationToken);
            if (ErrorCollector.HasError)
            {
                var genericResponse = await response.Content.ReadAsAsync<GenericResponse<object>>();
                if (ErrorCollector.Errors != null)
                {
                    genericResponse.Errors = ErrorCollector.Errors.ToArray();
                }

                return controllerContext.Request.CreateResponse(HttpStatusCode.OK, genericResponse);
            }

            return response;
        }
    }
}