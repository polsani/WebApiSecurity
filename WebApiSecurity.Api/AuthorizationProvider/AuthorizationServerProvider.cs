using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace WebApiSecurity.Api.AuthorizationProvider
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                if (VerifyAuthentication(context))
                {
                    context.SetError("invalid_grant", "Usuario ou senha inválidos");
                    return;
                }

                var identity = BuildUserPermissions(context);

                context.Validated(identity);
            }
            catch
            {
                context.SetError("invalid_grant", "Falha ao autenticar");
            }
        }

        private ClaimsIdentity BuildUserPermissions(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            var roles = new List<string> {"User"};

            roles.ForEach(x=> identity.AddClaim(new Claim(ClaimTypes.Role, x)));

            AddClaimsForUser(identity, context.UserName);
            
            var principal = new GenericPrincipal(identity, roles.ToArray());
            Thread.CurrentPrincipal = principal;
            return identity;
        }

        private void AddClaimsForUser(ClaimsIdentity identity, string userName)
        {
            if (userName == "henrique1")
                identity.AddClaim(new Claim("ClaimDefault", "ValueDefault"));
        }

        private static bool VerifyAuthentication(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return (context.UserName == "henrique" && context.Password == "123") ||
                   (context.UserName == "henrique1" && context.Password == "123");
        }
    }
}