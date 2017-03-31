using System;
using Windows.Security.Authentication.Web;

namespace SoftwareKobo.Social.SinaWeibo
{
    public class AuthorizationException : Exception
    {
        internal AuthorizationException(WebAuthenticationResult result)
        {
            Result = result;
        }

        public WebAuthenticationResult Result
        {
            get;
        }
    }
}