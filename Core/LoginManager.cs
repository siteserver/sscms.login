using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Models;
using SSCMS.Repositories;
using SSCMS.Services;
using SSCMS.Utils;

namespace SSCMS.Login.Core
{
    public class LoginManager : ILoginManager
    {
        public const string PluginId = "sscms.login";
        public const string PermissionsTemplates = "login_templates";
        public const string PermissionsLoginWeixin = "login_weixin";
        public const string PermissionsLoginQq = "login_qq";
        public const string PermissionsLoginWeibo = "login_weibo";

        private readonly IPathManager _pathManager;
        private readonly IPluginManager _pluginManager;
        private readonly IPluginConfigRepository _pluginConfigRepository;

        public LoginManager(IPathManager pathManager, IPluginManager pluginManager, IPluginConfigRepository pluginConfigRepository)
        {
            _pathManager = pathManager;
            _pluginManager = pluginManager;
            _pluginConfigRepository = pluginConfigRepository;
        }

        public async Task<WeixinSettings> GetWeixinSettingsAsync()
        {
            var settings = new WeixinSettings
            {
                IsWeixin =
                    await _pluginConfigRepository.GetAsync<bool>(PluginId,
                        nameof(WeixinSettings.IsWeixin)),
                WeixinAppId =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(WeixinSettings.WeixinAppId)),
                WeixinAppSecret =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(WeixinSettings.WeixinAppSecret))
            };
            return settings;
        }

        public async Task<QqSettings> GetQqSettingsAsync()
        {
            var settings = new QqSettings
            {
                IsQq =
                    await _pluginConfigRepository.GetAsync<bool>(PluginId,
                        nameof(QqSettings.IsQq)),
                QqAppId =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(QqSettings.QqAppId)),
                QqAppKey =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(QqSettings.QqAppKey)),
            };
            return settings;
        }

        public async Task<WeiboSettings> GetWeiboSettingsAsync()
        {
            var settings = new WeiboSettings
            {
                IsWeibo =
                    await _pluginConfigRepository.GetAsync<bool>(PluginId,
                        nameof(WeiboSettings.IsWeibo)),
                WeiboAppKey =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(WeiboSettings.WeiboAppKey)),
                WeiboAppSecret =
                    await _pluginConfigRepository.GetAsync<string>(PluginId,
                        nameof(WeiboSettings.WeiboAppSecret))
            };
            return settings;
        }

        public async Task SetWeixinSettingsAsync(WeixinSettings settings)
        {
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeixinSettings.IsWeixin), settings.IsWeixin);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeixinSettings.WeixinAppId), settings.WeixinAppId);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeixinSettings.WeixinAppSecret), settings.WeixinAppSecret);
        }

        public async Task SetQqSettingsAsync(QqSettings settings)
        {
            await _pluginConfigRepository.SetAsync(PluginId, nameof(QqSettings.IsQq), settings.IsQq);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(QqSettings.QqAppId), settings.QqAppId);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(QqSettings.QqAppKey), settings.QqAppKey);
        }

        public async Task SetWeiboSettingsAsync(WeiboSettings settings)
        {
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeiboSettings.IsWeibo), settings.IsWeibo);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeiboSettings.WeiboAppKey), settings.WeiboAppKey);
            await _pluginConfigRepository.SetAsync(PluginId, nameof(WeiboSettings.WeiboAppSecret), settings.WeiboAppSecret);
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

        private string GetTemplatesDirectoryPath()
        {
            var plugin = _pluginManager.GetPlugin(PluginId);
            return PathUtils.Combine(plugin.WebRootPath, "assets/login/templates");
        }

        public List<TemplateInfo> GetTemplateInfoList(string type)
        {
            var templateInfoList = new List<TemplateInfo>();

            var directoryPath = GetTemplatesDirectoryPath();
            var directoryNames = DirectoryUtils.GetDirectoryNames(directoryPath);
            foreach (var directoryName in directoryNames)
            {
                var templateInfo = GetTemplateInfo(directoryPath, directoryName);
                if (templateInfo == null) continue;
                if (StringUtils.EqualsIgnoreCase(type, templateInfo.Type))
                {
                    templateInfoList.Add(templateInfo);
                }
            }

            return templateInfoList;
        }

        public TemplateInfo GetTemplateInfo(string name)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            return GetTemplateInfo(directoryPath, name);
        }

        private TemplateInfo GetTemplateInfo(string templatesDirectoryPath, string name)
        {
            TemplateInfo templateInfo = null;

            var configPath = PathUtils.Combine(templatesDirectoryPath, name, "config.json");
            if (FileUtils.IsFileExists(configPath))
            {
                templateInfo = TranslateUtils.JsonDeserialize<TemplateInfo>(FileUtils.ReadText(configPath));
                templateInfo.Name = name;
            }

            return templateInfo;
        }

        public void Clone(string nameToClone, TemplateInfo templateInfo, string templateHtml = null)
        {
            var plugin = _pluginManager.GetPlugin(PluginId);
            var directoryPath = PathUtils.Combine(plugin.WebRootPath, "assets/login/templates");

            DirectoryUtils.Copy(PathUtils.Combine(directoryPath, nameToClone), PathUtils.Combine(directoryPath, templateInfo.Name), true);

            var configJson = TranslateUtils.JsonSerialize(templateInfo);
            var configPath = PathUtils.Combine(directoryPath, templateInfo.Name, "config.json");
            FileUtils.WriteText(configPath, configJson);

            if (templateHtml != null)
            {
                SetTemplateHtml(templateInfo, templateHtml);
            }
        }

        public void Edit(TemplateInfo templateInfo)
        {
            var plugin = _pluginManager.GetPlugin(PluginId);
            var directoryPath = PathUtils.Combine(plugin.ContentRootPath, "assets/login/templates");

            var configJson = TranslateUtils.JsonSerialize(templateInfo);
            var configPath = PathUtils.Combine(directoryPath, templateInfo.Name, "config.json");
            FileUtils.WriteText(configPath, configJson);
        }

        public string GetTemplateHtml(TemplateInfo templateInfo)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            var htmlPath = PathUtils.Combine(directoryPath, templateInfo.Name, templateInfo.Main);
            return _pathManager.GetContentByFilePath(htmlPath);
        }

        public void SetTemplateHtml(TemplateInfo templateInfo, string html)
        {
            var directoryPath = GetTemplatesDirectoryPath();
            var htmlPath = PathUtils.Combine(directoryPath, templateInfo.Name, templateInfo.Main);

            FileUtils.WriteText(htmlPath, html);
        }

        public void DeleteTemplate(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            var directoryPath = GetTemplatesDirectoryPath();
            var templatePath = PathUtils.Combine(directoryPath, name);
            DirectoryUtils.DeleteDirectoryIfExists(templatePath);
        }
    }
}
