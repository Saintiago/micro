using Owin;
using System.Web.Http;

namespace Circuit
{
    public class GoodPoemControllerStartup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{poemKey}",
                defaults: new { poemKey = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}