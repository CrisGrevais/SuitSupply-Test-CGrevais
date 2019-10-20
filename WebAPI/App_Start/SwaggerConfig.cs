using Swashbuckle.Application;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.App_Start;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPI.App_Start
{
    public static class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.PrettyPrint();
                    c.RootUrl(req => GetRootUrl(req));
                    c.SingleApiVersion("v1", "WebAPI");
                })
                .EnableSwaggerUi(c =>
                {
                });
        }

        private static string GetRootUrl(HttpRequestMessage req)
        {
            return $"https://{req.RequestUri.Authority}{System.Web.VirtualPathUtility.ToAbsolute("~/").TrimEnd('/')}";
        }
    }
}