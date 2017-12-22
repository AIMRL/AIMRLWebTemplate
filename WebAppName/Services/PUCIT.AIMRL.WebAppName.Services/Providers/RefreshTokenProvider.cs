using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace PUCIT.AIMRL.WebAppName.Services.Providers
{
    
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();
     
        //This method is used to create new Refresh Token and Access Token , when previous access token expires.
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var guid = Guid.NewGuid().ToString();

            //double tokenExpiryTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RefreshTokenExpiryDurationInMinutes"]);

            double tokenExpiryTime = 1;

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenExpiryTime)//DateTime.UtcNow.AddYears(1)  
            };
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenExpiryTime);
            // consider storing only the hash of the handle  
            context.SetToken(guid);  

            
        }

        void IAuthenticationTokenProvider.Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

       //This method is used to extract Authorization value which contain access token information from the header of request. 
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;
            string header = context.OwinContext.Request.Headers["Authorization"];

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }  
        }
    }

}