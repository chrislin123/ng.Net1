using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ng.Net1.Models;
using System.Diagnostics;

namespace ng.Net1.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Stopwatch sw;
            sw = new Stopwatch();    //Stopwatch類別在System.Diagnostics命名空間裡

            sw.Reset();
            sw = Stopwatch.StartNew();

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            sw.Stop();
            TimeSpan el = sw.Elapsed;
            System.Diagnostics.Debug.WriteLine("total time 1 : " + el.ToString());
            sw.Reset();
            sw = Stopwatch.StartNew();

            //UNDO:Check DB User Id & PW In Here!!!
            User user = await userManager.FindAsync(context.UserName, context.Password);

            sw.Stop();
            el = sw.Elapsed;
            System.Diagnostics.Debug.WriteLine("total time 2 : " + el.ToString());
            sw.Reset();
            sw = Stopwatch.StartNew();

            if (user == null)
            {
                context.SetError("invalid_grant", "帳號或密碼不正確，請重新輸入!");
                return;
            }

            sw.Stop();
            el = sw.Elapsed;
            System.Diagnostics.Debug.WriteLine("total time 3 : " + el.ToString());
            sw.Reset();
            sw = Stopwatch.StartNew();

            ClaimsIdentity oAuthIdentity   = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            //ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
            //    CookieAuthenticationDefaults.AuthenticationType);

            /*
            ClaimsIdentity oAuthIdentity = await user.CreateAsync(user,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.CreateAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);
            */

            sw.Stop();
            el = sw.Elapsed;
            System.Diagnostics.Debug.WriteLine("total time 4 : " + el.ToString());
            sw.Reset();
            sw = Stopwatch.StartNew();

            AuthenticationProperties properties = CreateProperties(user.UserName, user.Id.ToString());
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(oAuthIdentity);

            sw.Stop();
            el = sw.Elapsed;
            System.Diagnostics.Debug.WriteLine("total time 5 : " + el.ToString());
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName,string userId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "userId",userId }
            };
            return new AuthenticationProperties(data);
        }
    }
}