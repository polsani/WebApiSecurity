using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiSecurity.Api.Controllers
{
    public class DefaultController : ApiController
    {
        [Route("default/NotAuthenticatedService")]
        public HttpResponseMessage NotAuthenticatedService()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Response OK");
        }

        [Route("default/AuthenticatedService")]
        [Authorize]
        public HttpResponseMessage AuthenticatedService()
        {
            return Request.CreateResponse(HttpStatusCode.OK, User.Identity.Name);
        }

        [Route("default/AuthenticatedServiceWithClaims")]
        [ClaimsAuthorizate("ClaimDefault","ValueDefault")]
        public HttpResponseMessage AuthenticatedServiceWithClaims()
        {
            return Request.CreateResponse(HttpStatusCode.OK, User.Identity.Name);
        }
    }
}
