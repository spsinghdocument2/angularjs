using MAF.BAL;
using MAF.ENTITY;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MAF.WEBAPI.Provider
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            // return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
       

            LoginEntity objLoginEntity = new LoginEntity();
            objLoginEntity.Email = context.UserName;
            objLoginEntity.AccountNumber = context.Scope[0];
            objLoginEntity.Password = context.Password;
            if (string.IsNullOrWhiteSpace(context.Scope[0]) || string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
            {
                // Error message send  
                context.SetError(MAF.BAL.ResourceFile.Common.RequestInvalid);
                return;
            }
            Login login = new Login();
            objLoginEntity = login.UserLogin(objLoginEntity);
            if (objLoginEntity.IsSuccess == true)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("sub", context.UserName));

                var propertyDictionary = new Dictionary<string, string>();
                propertyDictionary.Add("ACNStatus", objLoginEntity.ACNStatus == null ? "" : objLoginEntity.ACNStatus);
                propertyDictionary.Add("IsSuccess", Convert.ToString(objLoginEntity.IsSuccess));
                propertyDictionary.Add("IsBitReset", Convert.ToString(objLoginEntity.IsBitReset));
                propertyDictionary.Add("Message", objLoginEntity.Message == null ? "" : objLoginEntity.Message);
                var properties = new AuthenticationProperties(propertyDictionary);

                var ticket = new AuthenticationTicket(identity, properties);
                context.Validated(ticket);
            }
            else
            {
                context.SetError(objLoginEntity.Message);
                return;
            }
        }

    

        public override async Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            //return Task.FromResult<object>(null);
        }

      
    }
}