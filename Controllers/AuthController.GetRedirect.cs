using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;
using SSCMS.Login.Models;
using SSCMS.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class AuthController
    {
        [HttpGet, Route(RouteRedirect)]
        public async Task<ActionResult<GetRedirectResult>> GetRedirect([FromRoute] string type,
            [FromQuery] GetRedirectRequest request)
        {
            var host = ApiUtils.GetHost(Request);
            var oAuthType = OAuthType.Parse(type);

            var userName = string.Empty;

            if (oAuthType == OAuthType.Weixin)
            {
                var settings = await _loginManager.GetWeixinSettingsAsync();
                var client = new WeixinClient(settings.WeixinAppId, settings.WeixinAppSecret, host, request.RedirectUrl);

                var userInfo = await client.GetUserInfoAsync(request.Code);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Weixin.Value, userInfo.UnionId);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User
                    {
                        UserName = await _userRepository.IsUserNameExistsAsync(userInfo.Nickname)
                            ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                            : userInfo.Nickname,
                        DisplayName = userInfo.Nickname,
                        AvatarUrl = userInfo.HeadImgUrl
                    };

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Weixin.Value,
                        UniqueId = userInfo.UnionId,
                        UserName = userName
                    });
                }
            }
            else if (oAuthType == OAuthType.Qq)
            {
                var settings = await _loginManager.GetQqSettingsAsync();
                var client = new QqClient(settings.QqAppId, settings.QqAppKey, host, request.RedirectUrl);

                var userInfo = await client.GetUserInfoAsync(request.Code);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Qq.Value, userInfo.UniqueId);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User
                    {
                        UserName = await _userRepository.IsUserNameExistsAsync(userInfo.DisplayName)
                            ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                            : userInfo.DisplayName,
                        DisplayName = userInfo.DisplayName,
                        AvatarUrl = userInfo.AvatarUrl
                    };

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Qq.Value,
                        UniqueId = userInfo.UniqueId,
                        UserName = userName
                    });
                }
            }
            else if (oAuthType == OAuthType.Weibo)
            {
                var settings = await _loginManager.GetWeiboSettingsAsync();
                var client = new WeiboClient(settings.WeiboAppKey, settings.WeiboAppSecret, host, request.RedirectUrl);

                var userInfo = await client.GetUserInfoAsync(request.Code);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Weibo.Value, userInfo.UnionId);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User();
                    user.UserName = await _userRepository.IsUserNameExistsAsync(userInfo.Name)
                        ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                        : userInfo.Name;
                    user.DisplayName = userInfo.ScreenName;
                    user.AvatarUrl = userInfo.AvatarLarge;

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Weibo.Value,
                        UniqueId = userInfo.UnionId,
                        UserName = userName
                    });
                }
            }

            var token = _authManager.AuthenticateUser(await _userRepository.GetByUserNameAsync(userName), true);

            if (oAuthType == OAuthType.Qq || oAuthType == OAuthType.Weibo)
            {
                return Redirect(PageUtils.AddQueryString(request.RedirectUrl, $"token={token}"));
            }
            else
            {
                return new GetRedirectResult
                {
                    RedirectUrl = request.RedirectUrl,
                    Token = token
                };
            }
        }
    }
}
