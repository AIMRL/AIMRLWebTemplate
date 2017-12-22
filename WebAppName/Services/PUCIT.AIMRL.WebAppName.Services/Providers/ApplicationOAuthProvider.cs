using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Net.Http;
using System.Threading;


namespace PUCIT.AIMRL.WebAppName.Services.Providers
{
    
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
       // Called to validate that the origin of the request is a registered "client_id".Currently we assume that client is always a registered client
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        //Called when a request to the Token endpoint arrives with a "grant_type" of
        //     "password". This occurs when the user has provided name and password credentials
        //     directly into the client application's user interface, and the client application
        //     is using those to acquire an "access_token" and optional "refresh_token".
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            
            //UserInfoDataRepository _repo = new UserInfoDataRepository();
            //ApiLogins user = _repo.GetLoginInfo(context.UserName, Utility.GetHash(context.Password)); //verifying user credentials from database

            //if (user == null)
            if(context.UserName != "test" || context.Password != "password")
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return Task.FromResult<object>(null);
            }
            

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

       //     Called when a request to the Token endpoint arrives with a "grant_type" of
        //     "refresh_token". This occurs if your application has issued a "refresh_token"
        //     along with the "access_token", and the client is attempting to use the "refresh_token"
        //     to acquire a new "access_token", and possibly a new "refresh_token". 
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
           

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        
        //     Called before the TokenEndpoint redirects its response to the caller.
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}