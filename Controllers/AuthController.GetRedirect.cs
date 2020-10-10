using System;
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
            var oAuthType = OAuthType.Parse(type);

            var userName = string.Empty;

            if (oAuthType == OAuthType.Weixin)
            {
                var settings = await _loginManager.GetWeixinSettingsAsync();
                var client = new WeixinClient(settings.WeixinAppId, settings.WeixinAppSecret, request.RedirectUrl);

                client.GetUserInfo(request.Code, out var nickname, out var headimgurl, out var unionid);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Weixin.Value, unionid);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User
                    {
                        UserName = await _userRepository.IsUserNameExistsAsync(nickname)
                            ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                            : nickname,
                        DisplayName = nickname,
                        AvatarUrl = headimgurl
                    };

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Weixin.Value,
                        UniqueId = unionid,
                        UserName = userName
                    });
                }
            }
            else if (oAuthType == OAuthType.Qq)
            {
                var settings = await _loginManager.GetQqSettingsAsync();
                var client = new QqClient(settings.QqAppId, settings.QqAppKey, request.RedirectUrl);

                client.GetUserInfo(request.Code, out var displayName, out var avatarUrl, out var uniqueId);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Qq.Value, uniqueId);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User
                    {
                        UserName = await _userRepository.IsUserNameExistsAsync(displayName)
                            ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                            : displayName,
                        DisplayName = displayName,
                        AvatarUrl = avatarUrl
                    };

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Qq.Value,
                        UniqueId = uniqueId,
                        UserName = userName
                    });
                }
            }
            else if (oAuthType == OAuthType.Weibo)
            {
                var settings = await _loginManager.GetWeiboSettingsAsync();
                var client = new WeiboClient(settings.WeiboAppKey, settings.WeiboAppSecret, request.RedirectUrl);

                client.GetUserInfo(request.Code, out var name, out var screenName, out var avatarLarge, out var uniqueId);

                userName = await _oAuthRepository.GetUserNameAsync(OAuthType.Weibo.Value, uniqueId);
                if (string.IsNullOrEmpty(userName))
                {
                    var user = new User();
                    user.UserName = await _userRepository.IsUserNameExistsAsync(name)
                        ? Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")
                        : name;
                    user.DisplayName = screenName;
                    user.AvatarUrl = avatarLarge;

                    var (newUser, _) = await _userRepository.InsertAsync(user, Guid.NewGuid().ToString(), PageUtils.GetIpAddress(Request));
                    userName = newUser.UserName;

                    await _oAuthRepository.InsertAsync(new OAuth
                    {
                        Source = OAuthType.Weibo.Value,
                        UniqueId = uniqueId,
                        UserName = userName
                    });
                }
            }

            var token = _authManager.AuthenticateUser(await _userRepository.GetByUserNameAsync(userName), true);
            return new GetRedirectResult
            {
                RedirectUrl = request.RedirectUrl,
                Token = token
            };
        }
    }
}
