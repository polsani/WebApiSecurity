using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApiSecurity.Api.AuthorizationProvider;

namespace WebApiSecurity.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            ConfigureApi(config);
            ConfigureOAuth(appBuilder);

            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);
        }

        public void ConfigureApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }

        public void ConfigureOAuth(IAppBuilder appBuilder)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new AuthorizationServerProvider()
            };

            appBuilder.UseOAuthAuthorizationServer(oAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}