using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiSecurity.Api
{
    public class ClaimsAuthorizateAttribute : AuthorizeAttribute
    {
        private readonly string _claimName;
        private readonly string _claimValue;

        public ClaimsAuthorizateAttribute(string claimName, string claimValue)
        {
            _claimName = claimName;
            _claimValue = claimValue;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if(user == null || !user.HasClaim(_claimName, _claimValue))
                HandleUnauthorizedRequest(actionContext);

            base.OnAuthorization(actionContext);
        }
    }
}