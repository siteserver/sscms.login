using System.Web;

namespace SSCMS.Login.Core
{
    public static class ApiUtils
    {
        public static string GetActionsLoginUrl()
        {
            return "/api/login/actions/login";
        }

        public static string GetActionsLogoutUrl()
        {
            return "/api/login/actions/logout";
        }

        public static string GetActionsRegisterUrl()
        {
            return "/api/login/actions/register";
        }

        public static string GetAuthUrl(OAuthType type)
        {
            return $"/api/login/auth/{type.Value}";
        }

        public static string GetAuthUrl(OAuthType type, string redirectUrl)
        {
            return $"/api/login/auth/{type.Value}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
        }

        public static string GetAuthRedirectUrl(OAuthType authType, string redirectUrl)
        {
            return $"/api/login/auth/{authType.Value}/redirect?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
        }

        public static string GetHomeUrl()
        {
            return "/home/";
        }
    }
}
