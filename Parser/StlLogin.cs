using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using SSCMS.Login.Core;
using SSCMS.Parse;
using SSCMS.Plugins;
using SSCMS.Services;
using SSCMS.Utils;

namespace SSCMS.Login.Parser
{
    public class StlLogin : IPluginParseAsync
    {
        public string ElementName => "stl:login";

        public const string AttributeType = "type";
        public const string AttributeRedirectUrl = "redirectUrl";

        public const string TypeAll = "all";

        private readonly IPluginManager _pluginManager;

        public StlLogin(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        public async Task<string> ParseAsync(IParseStlContext context)
        {
            var type = string.Empty;
            var redirectUrl = string.Empty;
            var attributes = new NameValueCollection();

            ParseUtils.RegisterBodyHtml(context, _pluginManager);

            foreach (var name in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[name];
                if (StringUtils.EqualsIgnoreCase(name, AttributeType))
                {
                    type = await context.ParseAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, AttributeRedirectUrl))
                {
                    redirectUrl = await context.ParseAsync(value);
                }
                else
                {
                    attributes.Add(name, await context.ParseAsync(value));
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "/home";
            }

            string text;
            var url = string.Empty;
            var onClick = string.Empty;
            if (StringUtils.EqualsIgnoreCase(type, OAuthType.Weibo.Value))
            {
                text = "微博登录";
                url = $"{ApiUtils.GetAuthUrl(OAuthType.Weibo)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
            }
            else if (StringUtils.EqualsIgnoreCase(type, OAuthType.Weixin.Value))
            {
                text = "微信登录";
                url = $"{ApiUtils.GetAuthUrl(OAuthType.Weixin)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
            }
            else if (StringUtils.EqualsIgnoreCase(type, OAuthType.Qq.Value))
            {
                text = "QQ登录";
                url = $"{ApiUtils.GetAuthUrl(OAuthType.Qq)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
            }
            else if (StringUtils.EqualsIgnoreCase(type, TypeAll))
            {
                text = "一键登录";
                onClick = ParseUtils.OnClickLoginAll;
            }
            else
            {
                text = "登录";
                onClick = ParseUtils.OnClickLogin;
            }

            attributes["href"] = string.IsNullOrEmpty(url) ? "javascript:;" : url;
            if (!string.IsNullOrEmpty(onClick))
            {
                attributes["onclick"] = onClick;
            }

            var innerHtml = await context.ParseAsync(context.StlInnerHtml);
            if (string.IsNullOrWhiteSpace(innerHtml))
            {
                innerHtml = text;
            }
            return $"<a {TranslateUtils.ToAttributesString(attributes)}>{innerHtml}</a>";
        }

        //public static object Login(IRequest request)
        //{
        //    var account = request.GetPostString("account");
        //    var password = request.GetPostString("password");

        //    IUserInfo user;
        //    string userName;
        //    string errorMessage;
        //    if (!Context.UserApi.Validate(account, password, out userName, out errorMessage))
        //    {
        //        user = Context.UserApi.GetUserInfoByUserName(userName);
        //        if (user != null)
        //        {
        //            user.CountOfFailedLogin += 1;
        //            user.LastActivityDate = DateTime.Now;
        //            Context.UserApi.Update(user);
        //        }
        //        throw new Exception(errorMessage);
        //    }

        //    user = Context.UserApi.GetUserInfoByUserName(userName);
        //    user.CountOfFailedLogin = 0;
        //    user.CountOfLogin += 1;
        //    user.LastActivityDate = DateTime.Now;
        //    Context.UserApi.Update(user);

        //    request.UserLogin(userName, true);

        //    return new
        //    {
        //        User = user
        //    };
        //}

        //public static HttpResponseMessage OAuth(IRequest context, OAuthType oAuthType)
        //{
        //    var configInfo = Utils.GetConfigInfo();
        //    var redirectUrl = context.GetQueryString("redirectUrl");
        //    if (string.IsNullOrEmpty(redirectUrl))
        //    {
        //        redirectUrl = configInfo.HomeUrl;
        //    }

        //    var url = string.Empty;

        //    if (oAuthType == OAuthType.Weibo)
        //    {
        //        var client = new WeiboClient(configInfo.WeiboAppKey, configInfo.WeiboAppSecret, redirectUrl);
        //        url = client.GetAuthorizationUrl();
        //    }
        //    else if (oAuthType == OAuthType.Weixin)
        //    {
        //        var client = new WeixinClient(configInfo.WeixinAppId, configInfo.WeixinAppSecret, redirectUrl);
        //        url = client.GetAuthorizationUrl();
        //    }
        //    else if (oAuthType == OAuthType.Qq)
        //    {
        //        var client = new QqClient(configInfo.QqAppId, configInfo.QqAppKey, redirectUrl);
        //        url = client.GetAuthorizationUrl();
        //    }

        //    if (!string.IsNullOrEmpty(url))
        //    {
        //        var response = new HttpResponseMessage
        //        {
        //            Content = new StringContent($"<script>location.href = '{url}';</script>"),
        //            StatusCode = HttpStatusCode.OK
        //        };
        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

        //        return response;
        //    }

        //    throw new Exception("类型不正确");
        //}

        //public static HttpResponseMessage OAuthRedirect(IRequest context, OAuthType oAuthType)
        //{
        //    var configInfo = Utils.GetConfigInfo();
        //    var redirectUrl = context.GetQueryString("redirectUrl");
        //    var code = context.GetQueryString("code");
        //    var userName = string.Empty;

        //    if (oAuthType == OAuthType.Weibo)
        //    {
        //        var client = new WeiboClient(configInfo.WeiboAppKey, configInfo.WeiboAppSecret, redirectUrl);

        //        string name;
        //        string screenName;
        //        string avatarLarge;
        //        string gender;
        //        string uniqueId;
        //        client.GetUserInfo(code, out name, out screenName, out avatarLarge, out gender, out uniqueId);

        //        userName = OAuthDao.GetUserName(OAuthType.Weibo.Value, uniqueId);
        //        if (string.IsNullOrEmpty(userName))
        //        {
        //            var user = Context.UserApi.NewInstance();
        //            user.UserName = Context.UserApi.IsUserNameExists(name) ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") : name;
        //            user.DisplayName = screenName;
        //            user.AvatarUrl = avatarLarge;
        //            user.Gender = gender;

        //            string errorMessage;
        //            Context.UserApi.Insert(user, Guid.NewGuid().ToString(), out errorMessage);
        //            userName = user.UserName;

        //            OAuthDao.Insert(new OAuthInfo
        //            {
        //                Source = OAuthType.Weibo.Value,
        //                UniqueId = uniqueId,
        //                UserName = userName
        //            });
        //        }
        //    }
        //    else if (oAuthType == OAuthType.Weixin)
        //    {
        //        var client = new WeixinClient(configInfo.WeixinAppId, configInfo.WeixinAppSecret, redirectUrl);

        //        string nickname;
        //        string headimgurl;
        //        string gender;
        //        string unionid;
        //        client.GetUserInfo(code, out nickname, out headimgurl, out gender, out unionid);

        //        userName = OAuthDao.GetUserName(OAuthType.Weixin.Value, unionid);
        //        if (string.IsNullOrEmpty(userName))
        //        {
        //            var user = Context.UserApi.NewInstance();
        //            user.UserName = Context.UserApi.IsUserNameExists(nickname) ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") : nickname;
        //            user.DisplayName = nickname;
        //            user.AvatarUrl = headimgurl;
        //            user.Gender = gender;

        //            string errorMessage;
        //            Context.UserApi.Insert(user, Guid.NewGuid().ToString(), out errorMessage);
        //            userName = user.UserName;

        //            OAuthDao.Insert(new OAuthInfo
        //            {
        //                Source = OAuthType.Weixin.Value,
        //                UniqueId = unionid,
        //                UserName = userName
        //            });
        //        }
        //    }
        //    else if (oAuthType == OAuthType.Qq)
        //    {
        //        var client = new QqClient(configInfo.QqAppId, configInfo.QqAppKey, redirectUrl);

        //        string displayName;
        //        string avatarUrl;
        //        string gender;
        //        string uniqueId;
        //        client.GetUserInfo(code, out displayName, out avatarUrl, out gender, out uniqueId);

        //        userName = OAuthDao.GetUserName(OAuthType.Qq.Value, uniqueId);
        //        if (string.IsNullOrEmpty(userName))
        //        {
        //            var user = Context.UserApi.NewInstance();
        //            user.UserName = Context.UserApi.IsUserNameExists(displayName) ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") : displayName;
        //            user.DisplayName = displayName;
        //            user.AvatarUrl = avatarUrl;
        //            user.Gender = gender;

        //            string errorMessage;
        //            Context.UserApi.Insert(user, Guid.NewGuid().ToString(), out errorMessage);
        //            userName = user.UserName;

        //            OAuthDao.Insert(new OAuthInfo
        //            {
        //                Source = OAuthType.Qq.Value,
        //                UniqueId = uniqueId,
        //                UserName = userName
        //            });
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        context.UserLogin(userName, true);
        //    }

        //    var response = new HttpResponseMessage
        //    {
        //        Content = new StringContent($"<script>location.href = '{redirectUrl}';</script>"),
        //        StatusCode = HttpStatusCode.OK
        //    };
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

        //    return response;
        //}
    }
}
