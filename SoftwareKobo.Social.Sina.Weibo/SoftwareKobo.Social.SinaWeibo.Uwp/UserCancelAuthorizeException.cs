using Windows.Security.Authentication.Web;

namespace SoftwareKobo.Social.SinaWeibo
{
    public sealed class UserCancelAuthorizeException : AuthorizationException
    {
        internal UserCancelAuthorizeException(WebAuthenticationResult result) : base(result)
        {
        }
    }
}