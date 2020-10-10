using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Models;
using SSCMS.Repositories;

namespace SSCMS.Login.Core
{
    public class LoginManager : ILoginManager
    {
        public const string PluginId = "sscms.login";
        public const string PermissionsLoginWeixin = "login_weixin";
        public const string PermissionsLoginQq = "login_qq";
        public const string PermissionsLoginWeibo = "login_weibo";

        private readonly IPluginConfigRepository _pluginConfigRepository;

        public LoginManager(IPluginConfigRepository pluginConfigRepository)
        {
            _pluginConfigRepository = pluginConfigRepository;
        }

        public async Task<WeixinSettings> GetWeixinSettingsAsync()
        {
            var settings = new WeixinSettings
            {
                IsWeixin =
                    await _pluginConfigRepository.GetConfigAsync<bool>(PluginId, 0,
                        nameof(WeixinSettings.IsWeixin)),
                WeixinAppId =
                    await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                        nameof(WeixinSettings.WeixinAppId)),
                WeixinAppSecret =
                await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                nameof(WeixinSettings.WeixinAppSecret))
            };
            return settings;
        }

        public async Task<QqSettings> GetQqSettingsAsync()
        {
            var settings = new QqSettings
            {
                IsQq =
                    await _pluginConfigRepository.GetConfigAsync<bool>(PluginId, 0,
                        nameof(QqSettings.IsQq)),
                QqAppId =
                    await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                        nameof(QqSettings.QqAppId)),
                QqAppKey =
                    await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                        nameof(QqSettings.QqAppKey)),
            };
            return settings;
        }

        public async Task<WeiboSettings> GetWeiboSettingsAsync()
        {
            var settings = new WeiboSettings
            {
                IsWeibo =
                    await _pluginConfigRepository.GetConfigAsync<bool>(PluginId, 0,
                        nameof(WeiboSettings.IsWeibo)),
                WeiboAppKey =
                    await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                        nameof(WeiboSettings.WeiboAppKey)),
                WeiboAppSecret =
                    await _pluginConfigRepository.GetConfigAsync<string>(PluginId, 0,
                        nameof(WeiboSettings.WeiboAppSecret))
            };
            return settings;
        }

        public async Task SetWeixinSettingsAsync(WeixinSettings settings)
        {
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeixinSettings.IsWeixin), settings.IsWeixin);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeixinSettings.WeixinAppId), settings.WeixinAppId);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeixinSettings.WeixinAppSecret), settings.WeixinAppSecret);
        }

        public async Task SetQqSettingsAsync(QqSettings settings)
        {
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(QqSettings.IsQq), settings.IsQq);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(QqSettings.QqAppId), settings.QqAppId);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(QqSettings.QqAppKey), settings.QqAppKey);
        }

        public async Task SetWeiboSettingsAsync(WeiboSettings settings)
        {
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeiboSettings.IsWeibo), settings.IsWeibo);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeiboSettings.WeiboAppKey), settings.WeiboAppKey);
            await _pluginConfigRepository.SetConfigAsync(PluginId, 0, nameof(WeiboSettings.WeiboAppSecret), settings.WeiboAppSecret);
        }

        public static string HttpGet(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";

                //响应
                var response = (HttpWebResponse)request.GetResponse();
                var text = string.Empty;
                using (var responseStm = response.GetResponseStream())
                {
                    if (responseStm != null)
                    {
                        var redStm = new StreamReader(responseStm, Encoding.UTF8);
                        text = redStm.ReadToEnd();
                    }
                }

                return text;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
